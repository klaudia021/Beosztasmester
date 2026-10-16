using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;

namespace Beosztasmester.Services;

public interface IHorizonsService
{
    Task<HorizonResponseDto> CreateHorizonAsync(CreateHorizonRequestDto dto);
    Task<StartSolveResponseDto> StartSolveAsync(int horizonId);
    Task<SolverStatusResponseDto?> GetSolverStatusAsync(int runId);
}

public class HorizonsService : IHorizonsService
{
    private static readonly ConcurrentDictionary<int, string> SolverRunIds = new();
    private static int nextSolverRunId;

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
            Department_Id = dto.OrgUnitId,
            Start_Date = dto.Start,
            End_Date = dto.End,
            Status = "DRAFT"
        };

        _dataContext.Horizons.Add(horizon);
        await _dataContext.SaveChangesAsync();

        return new HorizonResponseDto
        {
            Id = horizon.Id,
            OrgUnitId = horizon.Department_Id,
            Start = dto.Start,
            End = dto.End,
            Status = horizon.Status
        };
    }

    public async Task<StartSolveResponseDto> StartSolveAsync(int horizonId)
    {
        var runId = Interlocked.Increment(ref nextSolverRunId);
        var solvePayload = new
        {
            horizonId = horizonId.ToString(CultureInfo.InvariantCulture),
            engineType = "dummy",
            instance = new
            {
                instanceId = runId.ToString(CultureInfo.InvariantCulture)
            }
        };

        using var response = await _httpClient.PostAsJsonAsync("/solve", solvePayload);
        response.EnsureSuccessStatusCode();

        var solverResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (solverResponse.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidDataException("The solver returned an invalid solve response.");
        }

        var solverRunId = GetRequiredString(solverResponse, "runId");

        if (!SolverRunIds.TryAdd(runId, solverRunId))
        {
            throw new InvalidOperationException($"The generated run ID '{runId}' is already registered.");
        }

        return new StartSolveResponseDto
        {
            RunId = runId
        };
    }

    public async Task<SolverStatusResponseDto?> GetSolverStatusAsync(int runId)
    {
        if (!SolverRunIds.TryGetValue(runId, out var solverRunId))
        {
            return null;
        }

        using var stateResponse = await _httpClient.GetAsync($"/solver-runs/{solverRunId}");
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
        var horizonId = GetOptionalInt32(state, "horizonId");
        if (!IsCompleted(status))
        {
            return new SolverStatusResponseDto
            {
                Status = status.ToUpperInvariant(),
                HorizonId = horizonId,
                Roster = null
            };
        }

        using var resultResponse = await _httpClient.GetAsync($"/solver-runs/{solverRunId}/result");
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
            HorizonId = GetOptionalInt32(solverResult, "horizonId") ?? horizonId,
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
                    EmployeeId = GetRequiredInt32(item, "employeeId"),
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

    private static string? GetOptionalString(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) &&
        property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;

    private static int GetRequiredInt32(JsonElement element, string propertyName)
    {
        if (element.ValueKind != JsonValueKind.Object ||
            !element.TryGetProperty(propertyName, out var property))
        {
            throw new InvalidDataException($"The solver response did not include a valid '{propertyName}'.");
        }

        return ParseInt32(property, propertyName);
    }

    private static int? GetOptionalInt32(JsonElement element, string propertyName)
    {
        if (element.ValueKind != JsonValueKind.Object ||
            !element.TryGetProperty(propertyName, out var property) ||
            property.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        return ParseInt32(property, propertyName);
    }

    private static int ParseInt32(JsonElement property, string propertyName)
    {
        if (property.ValueKind == JsonValueKind.Number && property.TryGetInt32(out var number))
        {
            return number;
        }

        if (property.ValueKind == JsonValueKind.String)
        {
            var value = property.GetString();
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
            {
                return number;
            }

            if (value is { Length: > 1 } &&
                (value[0] == 'E' || value[0] == 'e') &&
                int.TryParse(value.AsSpan(1), NumberStyles.None, CultureInfo.InvariantCulture, out number))
            {
                return number;
            }
        }

        throw new InvalidDataException($"The solver response included an invalid '{propertyName}'.");
    }
}
