using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace Beosztasmester.Services;

public interface IHorizonsService
{
    Task<HorizonResponseDto> CreateHorizonAsync(CreateHorizonRequestDto dto);
    Task<StartSolveResponseDto?> StartSolveAsync(string horizonId);
    Task<SolverStatusResponseDto?> GetSolverStatusAsync(string runId);
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
            Id = Guid.NewGuid(),
            Department_Id = dto.OrgUnitId,
            Start_Date = dto.Start,
            End_Date = dto.End,
            Status = "DRAFT"
        };

        _dataContext.Horizons.Add(horizon);
        await _dataContext.SaveChangesAsync();

        return new HorizonResponseDto
        {
            Id = horizon.Id.ToString(),
            OrgUnitId = horizon.Department_Id,
            Start = dto.Start,
            End = dto.End,
            Status = horizon.Status
        };
    }

    public async Task<StartSolveResponseDto?> StartSolveAsync(string horizonId)
    {
        if (!Guid.TryParse(horizonId, out var id) ||
            !await _dataContext.Horizons.AnyAsync(horizon => horizon.Id == id))
        {
            return null;
        }

        var solvePayload = new
        {
            horizonId,
            engineType = "dummy",
            instance = new
            {
                instanceId = $"inst-{horizonId}"
            }
        };

        using var response = await _httpClient.PostAsJsonAsync("/solve", solvePayload);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<StartSolveResponseDto>()
            ?? throw new InvalidDataException("The solver returned an empty solve response.");
    }

    public async Task<SolverStatusResponseDto?> GetSolverStatusAsync(string runId)
    {
        using var response = await _httpClient.GetAsync($"/solver-runs/{runId}/result");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var solverResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (solverResponse.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidDataException("The solver returned an invalid status response.");
        }

        var status = solverResponse.GetProperty("status").GetString()
            ?? throw new InvalidDataException("The solver status response did not include a status.");

        var statusDto = new SolverStatusResponseDto
        {
            Status = status.ToUpperInvariant(),
            HorizonId = solverResponse.TryGetProperty("horizonId", out var horizonId)
                ? horizonId.GetString()
                : null,
            Roster = new List<RosterItemDto>()
        };

        if (solverResponse.TryGetProperty("result", out var result) &&
            result.ValueKind == JsonValueKind.Object &&
            result.TryGetProperty("assignments", out var assignments))
        {
            if (assignments.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidDataException("The solver assignments value must be an array.");
            }

            foreach (var item in assignments.EnumerateArray())
            {
                statusDto.Roster.Add(new RosterItemDto
                {
                    EmployeeId = item.GetProperty("employeeId").GetString()
                        ?? throw new InvalidDataException("A solver assignment did not include an employee ID."),
                    Date = item.GetProperty("date").GetDateTime(),
                    ShiftType = item.GetProperty("shiftType").GetString()
                        ?? throw new InvalidDataException("A solver assignment did not include a shift type.")
                });
            }
        }

        return statusDto;
    }
}
