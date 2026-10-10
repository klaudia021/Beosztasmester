using Beosztasmester.Models;
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
    [HttpPost("{id}/solve")]
    public async Task<ActionResult<StartSolveResponseDto>> StartSolve(string id)
    {
        var result = await _horizonsService.StartSolveAsync(id);
        if (result == null)
        {
            return NotFound(new { error = "not found" });
        }

        return StatusCode(202, result);
    }

    /// <summary>
    /// GET /api/status/{runId} — Solver állapot lekérdezése (Polling)
    /// </summary>
    [HttpGet("{runId}")]
    public async Task<ActionResult<SolverStatusResponseDto>> GetStatus(string runId)
    {
        var result = await _horizonsService.GetSolverStatusAsync(runId);
        if (result == null)
        {
            return NotFound(new { error = "not found" });
        }

        return Ok(result);
    }
}