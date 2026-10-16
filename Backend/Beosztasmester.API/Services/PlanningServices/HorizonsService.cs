using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Beosztasmester.Services;

public interface IHorizonsService
{
    Task<HorizonResponseDto> CreateHorizonAsync(CreateHorizonRequestDto dto);
    Task<StartSolveResponseDto?> StartSolveAsync(int horizonId);
    Task<SolverStatusResponseDto?> GetSolverStatusAsync(int runId);
}

public class HorizonsService : IHorizonsService
{
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
        await _dataContext.SaveChangesAsync();   // PostgreSQL generates horizon.Id here

        return new HorizonResponseDto
        {
            Id = horizon.Id,
            OrgUnitId = horizon.DepartmentId,
            Start = dto.Start,
            End = dto.End,
            Status = horizon.Status
        };
    }

    public async Task<StartSolveResponseDto?> StartSolveAsync(int horizonId)
    {
        if (!await _dataContext.Horizons.AnyAsync(h => h.Id == horizonId))
        {
            return null;
        }

        var run = new SolverRun { HorizonId = horizonId, Status = "RUNNING" };
        _dataContext.SolverRuns.Add(run);
        await _dataContext.SaveChangesAsync();   // PostgreSQL generates run.Id here

        try
        {
            await SendToSolverAsync(horizonId, run.Id);
        }
        catch
        {
            run.Status = "FAILED";
            await _dataContext.SaveChangesAsync();
            throw;
        }

        return new StartSolveResponseDto { RunId = run.Id };
    }

    private async Task SendToSolverAsync(int horizonId, int runId)
    {
        var payload = new
        {
            horizonId,
            engineType = "dummy",
            instance = new { instanceId = runId }
        };

        using var response = await _httpClient.PostAsJsonAsync("/solve", payload);
        response.EnsureSuccessStatusCode();
    }

    public async Task<SolverStatusResponseDto?> GetSolverStatusAsync(int runId)
    {
        using var stateResponse = await _httpClient.GetAsync($"/solver-runs/{runId}");
        if (!stateResponse.IsSuccessStatusCode)
        {
            if (stateResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            stateResponse.EnsureSuccessStatusCode();
        }

        var state = await stateResponse.Content.ReadFromJsonAsync<JsonElement>();
        if (state.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidDataException("The solver returned an invalid state response.");
        }

        var status = GetRequiredString(state, "status");
        var horizonId = GetOptionalInt(state, "horizonId");
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
        if (resultResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        resultResponse.EnsureSuccessStatusCode();

        var solverResult = await resultResponse.Content.ReadFromJsonAsync<JsonElement>();
        if (solverResult.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidDataException("The solver returned an invalid result response.");
        }

        var statusDto = new SolverStatusResponseDto
        {
            Status = "DONE",
            HorizonId = GetOptionalInt(solverResult, "horizonId") ?? horizonId,
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
                    EmployeeId = Int32.Parse(GetRequiredString(item, "employeeId")),
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
                    EmployeeId = 1,
                    Date = new DateOnly(2026, 3, 1),
                    ShiftType = "D"
                },
                new RosterItemDto
                {
                    EmployeeId = 2,
                    Date = new DateOnly(2026, 3, 1),
                    ShiftType = "N"
                }
            ];
        }

        return statusDto;
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

    private static int? GetOptionalInt(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) &&
        property.ValueKind == JsonValueKind.Number &&
        property.TryGetInt32(out var value)
            ? value
            : null;
}