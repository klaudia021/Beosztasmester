using Beosztasmester.Model;
using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;

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

    public HorizonsService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<HorizonResponseDto> CreateHorizonAsync(CreateHorizonRequestDto dto)
    {
        var horizon = new Horizon
        {
            Id = Guid.NewGuid(),
            Department_Id = dto.OrgUnitId,
            Start_Date = DateTime.SpecifyKind(DateTime.Parse(dto.Start), DateTimeKind.Utc),
            End_Date = DateTime.SpecifyKind(DateTime.Parse(dto.End), DateTimeKind.Utc),
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

        var horizon = await _dataContext.Horizons.FindAsync(id);
        if (horizon == null) return null;

        // Generálunk egy egyedi runId azonosítót a solver futásnak
        string runId = $"run-{Guid.NewGuid().ToString().Substring(0, 8)}";

        // TODO: Az OR-Tools Solver elindítása a háttérben (BackgroundService / Task)

        return new StartSolveResponseDto { RunId = runId };
    }

    public async Task<SolverStatusResponseDto?> GetSolverStatusAsync(string runId)
    {
        // 1. Ha a runId nem létezik a nyilvántartásban:
        // return null;

        // 2. Szimulált válasz a V1 JSON szerződés szerint:
        return await Task.FromResult(new SolverStatusResponseDto
        {
            Status = "DONE",
            HorizonId = "121",
            Roster = new List<RosterItemDto>
            {
                new RosterItemDto { Employee_Id = "E001", Date = "2026-03-01", Shift_Type = "D" },
                new RosterItemDto { Employee_Id = "E002", Date = "2026-03-01", Shift_Type = "N" }
            }
        });
    }
}
