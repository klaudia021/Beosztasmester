using Beosztasmester.Models.DTOs;
using Beosztasmester.Services;
using Microsoft.AspNetCore.Mvc;

namespace Beosztasmester.Controllers.Planning;

[ApiController]
[Route("api/horizons")]
public class HorizonsController : ControllerBase
{
    private readonly IHorizonsService _horizonsService;

    public HorizonsController(IHorizonsService horizonsService)
    {
        _horizonsService = horizonsService;
    }

    /// <summary>
    /// POST /api/horizons — Új horizont létrehozása
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<HorizonResponseDto>> CreateHorizon([FromBody] CreateHorizonRequestDto dto)
    {
        var result = await _horizonsService.CreateHorizonAsync(dto);
        return CreatedAtAction(nameof(CreateHorizon), new { id = result.Id }, result);
    }

    /// <summary>
    /// POST /api/horizons/{id}/solve — Solver indítása (202 Accepted)
    /// </summary>
    [HttpPost("{id:int}/solve")]
    public async Task<ActionResult<StartSolveResponseDto>> StartSolve(int id)
    {
        var result = await _horizonsService.StartSolveAsync(id);
        if (result == null)
        {
            return NotFound(new { error = $"Entity not found with id '{id}' - StartSolve function" });
        }

        return Accepted(result);
    }

    /// <summary>
    /// GET /api/horizons/{runId} — Solver állapot lekérdezése (Polling)
    /// </summary>
    [HttpGet("{runId:int}")]
    public async Task<ActionResult<SolverStatusResponseDto>> GetStatus(int runId)
    {
        var result = await _horizonsService.GetSolverStatusAsync(runId);

        if (result == null)
        {
            return NotFound(new
            {
                error = $"Solver run process not found with runId '{runId}' - GetStatus function"
            });
        }

        return Ok(result);
    }
}