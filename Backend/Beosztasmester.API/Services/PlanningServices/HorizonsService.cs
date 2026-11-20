using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;

namespace Beosztasmester.Services;

public interface IHorizonsService
{
    Task<HorizonResponseDto> CreateHorizonAsync(CreateHorizonRequestDto dto);
    Task<StartSolveResponseDto?> StartSolveAsync(Guid horizonId);
    Task<SolverStatusResponseDto?> GetSolverStatusAsync(Guid runId);
}

public class HorizonsService : IHorizonsService
{
    private static readonly SemaphoreSlim LogLock = new(1, 1);
    private static readonly JsonSerializerOptions LogJsonOptions = new() { WriteIndented = true };

    private readonly DataContext _dataContext;
    private readonly HttpClient _httpClient;

    public HorizonsService(DataContext dataContext, HttpClient httpClient)
    {
        _dataContext = dataContext;
        _httpClient = httpClient;
    }

    public async Task<HorizonResponseDto> CreateHorizonAsync(CreateHorizonRequestDto dto)
    {
        var horizon = new Horizon
        {
            DepartmentId = dto.OrgUnitId,
            StartDate = dto.Start,
            EndDate = dto.End,
            Status = "DRAFT"
        };

        _dataContext.Horizons.Add(horizon);
        try
        {
            await _dataContext.SaveChangesAsync();   // PostgreSQL generates horizon.Id here
        }
        catch (Exception exception)
        {
            await LogAsync("Create horizon failed", dto, new { exception = exception.ToString() });
            throw;
        }

        var response = new HorizonResponseDto
        {
            Id = horizon.Id,
            OrgUnitId = horizon.DepartmentId,
            Start = dto.Start,
            End = dto.End,
            Status = horizon.Status
        };

        return response;
    }

    public async Task<StartSolveResponseDto?> StartSolveAsync(Guid horizonId)
    {
        if (!await _dataContext.Horizons.AnyAsync(h => h.Id == horizonId))
        {
            await LogAsync("Start solve horizon not found", new { horizonId },
                new { statusCode = StatusCodes.Status404NotFound });
            return null;
        }

        var run = new SolverRun { HorizonId = horizonId, Status = "RUNNING" };
        _dataContext.SolverRuns.Add(run);
        await _dataContext.SaveChangesAsync();   // PostgreSQL generates run.Id here

        try
        {
            await SendToSolverAsync(horizonId, run.Id);
        }
        catch (Exception exception)
        {
            run.Status = "FAILED";
            await _dataContext.SaveChangesAsync();
            await LogAsync("Start solve failed", new { horizonId, runId = run.Id },
                new { exception = exception.ToString() });
            throw;
        }

        return new StartSolveResponseDto { RunId = run.Id };
    }

    private async Task SendToSolverAsync(Guid horizonId, Guid runId)
    {
        var payload = new
        {
            runId,
            horizonId,
            engineType = "dummy",
            instance = new { instanceId = runId }
        };

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync("/solve", payload);
        }
        catch (Exception exception)
        {
            await LogAsync("Solver request failed", new { endpoint = "/solve", payload },
                new { exception = exception.ToString() });
            throw;
        }

        using (response)
        {
        var responseBody = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            await LogAsync("Solver returned an error", new { endpoint = "/solve", payload },
                new { statusCode = (int)response.StatusCode, body = responseBody });
        }
        response.EnsureSuccessStatusCode();
        }
    }

    public async Task<SolverStatusResponseDto?> GetSolverStatusAsync(Guid runId)
    {
        object? solverResponse = null;
        try
        {
            using var stateResponse = await _httpClient.GetAsync($"/solver-runs/{runId}");
            var stateBody = await stateResponse.Content.ReadAsStringAsync();
            solverResponse = new
            {
                endpoint = $"/solver-runs/{runId}",
                statusCode = (int)stateResponse.StatusCode,
                body = stateBody
            };

            if (!stateResponse.IsSuccessStatusCode)
            {
                if (stateResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await LogAsync("Solver run not found", new { runId }, solverResponse);
                    return null;
                }

                stateResponse.EnsureSuccessStatusCode();
            }

            var state = JsonSerializer.Deserialize<JsonElement>(stateBody);
            if (state.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidDataException("The solver returned an invalid state response.");
            }

            var status = GetRequiredString(state, "status");
            var horizonId = GetOptionalGuid(state, "horizonId");
            if (!IsCompleted(status))
            {
                return new SolverStatusResponseDto
                {
                    Status = status.ToUpperInvariant(),
                    HorizonId = horizonId,
                    Roster = null
                };
            }

            using var resultResponse = await _httpClient.GetAsync($"/solver-runs/{runId}/result");
            var resultBody = await resultResponse.Content.ReadAsStringAsync();
            solverResponse = new
            {
                endpoint = $"/solver-runs/{runId}/result",
                statusCode = (int)resultResponse.StatusCode,
                body = resultBody
            };

            if (resultResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await LogAsync("Solver result not found", new { runId }, solverResponse);
                return null;
            }

            resultResponse.EnsureSuccessStatusCode();

            var solverResult = JsonSerializer.Deserialize<JsonElement>(resultBody);
            if (solverResult.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidDataException("The solver returned an invalid result response.");
            }

            var statusDto = new SolverStatusResponseDto
            {
                Status = "DONE",
                HorizonId = GetOptionalGuid(solverResult, "horizonId") ?? horizonId,
                Roster = new List<RosterItemDto>()
            };

            var assignments = solverResult.TryGetProperty("result", out var result)
                ? GetAssignments(result)
                : GetAssignments(solverResult);
            if (assignments.ValueKind != JsonValueKind.Undefined &&
                assignments.ValueKind != JsonValueKind.Null)
            {
                if (assignments.ValueKind != JsonValueKind.Array)
                {
                    throw new InvalidDataException("The solver assignments value must be an array.");
                }

                foreach (var item in assignments.EnumerateArray())
                {
                    statusDto.Roster.Add(new RosterItemDto
                    {
                        EmployeeId = GetRequiredGuid(item, "employeeId"),
                        Date = DateOnly.Parse(GetRequiredString(item, "date")),
                        ShiftType = GetRequiredString(item, "shiftType")
                    });
                }
            }

            if (statusDto.Roster.Count == 0)
            {
                statusDto.Roster =
                [
                    new RosterItemDto
                    {
                        EmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        Date = new DateOnly(2026, 3, 1),
                        ShiftType = "D"
                    },
                    new RosterItemDto
                    {
                        EmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        Date = new DateOnly(2026, 3, 1),
                        ShiftType = "N"
                    }
                ];
            }

            return statusDto;
        }
        catch (Exception exception)
        {
            await LogAsync("Get solver status failed", new { runId },
                new { solverResponse, exception = exception.ToString() });
            throw;
        }
    }

    private static async Task LogAsync(string eventName, object? input, object? output)
    {
        var timestamp = DateTimeOffset.Now;
        var formattedTimestamp = timestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        var entry = $"{formattedTimestamp} | {eventName}{Environment.NewLine}" +
                    $"Input: {JsonSerializer.Serialize(input, LogJsonOptions)}{Environment.NewLine}" +
                    $"Output: {JsonSerializer.Serialize(output, LogJsonOptions)}{Environment.NewLine}" +
                    Environment.NewLine;
        var directory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        var filePath = Path.Combine(directory, $"{timestamp:yyyy-MM-dd}.txt");

        await LogLock.WaitAsync();
        try
        {
            Directory.CreateDirectory(directory);
            await File.AppendAllTextAsync(filePath, entry);
        }
        finally
        {
            LogLock.Release();
        }
    }

    private static bool IsCompleted(string status) =>
        status.Equals("DONE", StringComparison.OrdinalIgnoreCase) ||
        status.Equals("COMPLETED", StringComparison.OrdinalIgnoreCase);

    private static JsonElement GetAssignments(JsonElement result)
    {
        if (result.ValueKind == JsonValueKind.Array)
        {
            return result;
        }

        if (result.ValueKind != JsonValueKind.Object)
        {
            return default;
        }

        if (result.TryGetProperty("assignments", out var assignments) ||
            result.TryGetProperty("roster", out assignments))
        {
            return assignments;
        }

        return default;
    }

    private static string GetRequiredString(JsonElement element, string propertyName)
    {
        if (element.ValueKind != JsonValueKind.Object ||
            !element.TryGetProperty(propertyName, out var property) ||
            property.ValueKind != JsonValueKind.String ||
            string.IsNullOrWhiteSpace(property.GetString()))
        {
            throw new InvalidDataException($"The solver response did not include a valid '{propertyName}'.");
        }

        return property.GetString()
            ?? throw new InvalidDataException($"The solver response did not include a valid '{propertyName}'.");
    }

    private static Guid GetRequiredGuid(JsonElement element, string propertyName)
    {
        if (element.ValueKind != JsonValueKind.Object ||
            !element.TryGetProperty(propertyName, out var property) ||
            property.ValueKind != JsonValueKind.String ||
            !Guid.TryParse(property.GetString(), out var value))
        {
            throw new InvalidDataException($"The solver response did not include a valid GUID '{propertyName}'.");
        }

        return value;
    }

    private static Guid? GetOptionalGuid(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) &&
        property.ValueKind == JsonValueKind.String &&
        Guid.TryParse(property.GetString(), out var value)
            ? value
            : null;
}