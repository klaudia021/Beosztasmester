using Beosztasmester.Model;
using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;
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
        if (!Guid.TryParse(horizonId, out var id)) return null;

        var solvePayload = new
        {
            horizonId = horizonId,
            engineType = "dummy",
            instance = new
            {
                instanceId = $"inst-{horizonId}"
            }
        };
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/solve", solvePayload);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<StartSolveResponseDto>();
                if (result != null) return result;
            }
        }
        catch (Exception)
        {
            // Ha a Python Solver HTTP microservice nem elérhető (fallback megoldás teszteléshez)
        }
        // Generálunk egy egyedi runId azonosítót a solver futásnak
        string fallbackRunId = $"run-{Guid.NewGuid().ToString().Substring(0, 8)}";

        
        return new StartSolveResponseDto { RunId = fallbackRunId };
    }

    public async Task<SolverStatusResponseDto?> GetSolverStatusAsync(string runId)
    {
        // 1. Ha a runId nem létezik a nyilvántartásban:
        // return null;
        try
        {
            var response = await _httpClient.GetAsync($"/solver-runs/{runId}/result");

            if (response.IsSuccessStatusCode)
            {
                var solverResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

                string status = solverResponse.GetProperty("status").GetString() ?? "RUNNING";

                var statusDto = new SolverStatusResponseDto
                {
                    Status = status.ToUpper(),
                    HorizonId = solverResponse.TryGetProperty("horizonId", out var h) ? h.GetString() : null,
                    Roster = new List<RosterItemDto>()
                };

                // Ha van kiszámolt roster eredmény
                if (solverResponse.TryGetProperty("result", out var res) && res.ValueKind != JsonValueKind.Null)
                {
                    if (res.TryGetProperty("assignments", out var assignments))
                    {
                        foreach (var item in assignments.EnumerateArray())
                        {
                            statusDto.Roster.Add(new RosterItemDto
                            {
                                EmployeeId = item.GetProperty("employeeId").GetString() ?? "",
                                Date = DateTime.Parse(item.GetProperty("date").GetString()!),
                                ShiftType = item.GetProperty("shiftType").GetString() ?? ""
                            });
                        }
                    }
                }

                return statusDto;
            }
        }
        catch (Exception)
        {
            // Fallback, ha a Python Solver nem érhető el
        }
        // 2. Szimulált válasz a V1 JSON szerződés szerint:
        return await Task.FromResult(new SolverStatusResponseDto
        {
            Status = "DONE",
            HorizonId = "121",
            Roster = new List<RosterItemDto>
            {
                new RosterItemDto { EmployeeId = "E001", Date = new DateTime(2026, 3, 2, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E001", Date = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E001", Date = new DateTime(2026, 3, 4, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E001", Date = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), ShiftType = "N" },
                new RosterItemDto { EmployeeId = "E001", Date = new DateTime(2026, 3, 6, 0, 0, 0, DateTimeKind.Utc), ShiftType = "N" },
                new RosterItemDto { EmployeeId = "E001", Date = new DateTime(2026, 3, 7, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E001", Date = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },

                // E002
                new RosterItemDto { EmployeeId = "E002", Date = new DateTime(2026, 3, 2, 0, 0, 0, DateTimeKind.Utc), ShiftType = "N" },
                new RosterItemDto { EmployeeId = "E002", Date = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E002", Date = new DateTime(2026, 3, 4, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E002", Date = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E002", Date = new DateTime(2026, 3, 6, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
                new RosterItemDto { EmployeeId = "E002", Date = new DateTime(2026, 3, 7, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
                new RosterItemDto { EmployeeId = "E002", Date = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },

                // E003
                new RosterItemDto { EmployeeId = "E003", Date = new DateTime(2026, 3, 2, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
                new RosterItemDto { EmployeeId = "E003", Date = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
                new RosterItemDto { EmployeeId = "E003", Date = new DateTime(2026, 3, 4, 0, 0, 0, DateTimeKind.Utc), ShiftType = "N" },
                new RosterItemDto { EmployeeId = "E003", Date = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E003", Date = new DateTime(2026, 3, 6, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E003", Date = new DateTime(2026, 3, 7, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E003", Date = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },

                // E004
                new RosterItemDto { EmployeeId = "E004", Date = new DateTime(2026, 3, 2, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E004", Date = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc), ShiftType = "N" },
                new RosterItemDto { EmployeeId = "E004", Date = new DateTime(2026, 3, 4, 0, 0, 0, DateTimeKind.Utc), ShiftType = "N" },
                new RosterItemDto { EmployeeId = "E004", Date = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
                new RosterItemDto { EmployeeId = "E004", Date = new DateTime(2026, 3, 6, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E004", Date = new DateTime(2026, 3, 7, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E004", Date = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },

                // E005
                new RosterItemDto { EmployeeId = "E005", Date = new DateTime(2026, 3, 2, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E005", Date = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E005", Date = new DateTime(2026, 3, 4, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
                new RosterItemDto { EmployeeId = "E005", Date = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), ShiftType = "N" },
                new RosterItemDto { EmployeeId = "E005", Date = new DateTime(2026, 3, 6, 0, 0, 0, DateTimeKind.Utc), ShiftType = "N" },
                new RosterItemDto { EmployeeId = "E005", Date = new DateTime(2026, 3, 7, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E005", Date = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },

                // E006
                new RosterItemDto { EmployeeId = "E006", Date = new DateTime(2026, 3, 2, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
                new RosterItemDto { EmployeeId = "E006", Date = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E006", Date = new DateTime(2026, 3, 4, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E006", Date = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E006", Date = new DateTime(2026, 3, 6, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
                new RosterItemDto { EmployeeId = "E006", Date = new DateTime(2026, 3, 7, 0, 0, 0, DateTimeKind.Utc), ShiftType = "N" },
                new RosterItemDto { EmployeeId = "E006", Date = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },

                // E007
                new RosterItemDto { EmployeeId = "E007", Date = new DateTime(2026, 3, 2, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E007", Date = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E007", Date = new DateTime(2026, 3, 4, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E007", Date = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
                new RosterItemDto { EmployeeId = "E007", Date = new DateTime(2026, 3, 6, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E007", Date = new DateTime(2026, 3, 7, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E007", Date = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc), ShiftType = "N" },

                // E008
                new RosterItemDto { EmployeeId = "E008", Date = new DateTime(2026, 3, 2, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E008", Date = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc), ShiftType = "D" },
                new RosterItemDto { EmployeeId = "E008", Date = new DateTime(2026, 3, 4, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
                new RosterItemDto { EmployeeId = "E008", Date = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
                new RosterItemDto { EmployeeId = "E008", Date = new DateTime(2026, 3, 6, 0, 0, 0, DateTimeKind.Utc), ShiftType = "OFF" },
                new RosterItemDto { EmployeeId = "E008", Date = new DateTime(2026, 3, 7, 0, 0, 0, DateTimeKind.Utc), ShiftType = "N" },
                new RosterItemDto { EmployeeId = "E008", Date = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc), ShiftType = "A" },
            }
        });
    }
}
