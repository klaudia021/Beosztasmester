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
        try
        {
            var result = await _horizonsService.CreateHorizonAsync(dto);
            return CreatedAtAction(nameof(CreateHorizon), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = $"Failed to create Horizon - CreateHorizon function. Details: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// POST /api/horizons/{id}/solve — Solver indítása (202 Accepted)
    /// </summary>
    [HttpPost("{id}/solve")]
    public async Task<ActionResult<StartSolveResponseDto>> StartSolve(string id)
    {
        try
        {
            var result = await _horizonsService.StartSolveAsync(id);
            if (result == null)
            {
                return NotFound(new
                {
                    error = $"Entity not found with id '{id}' - StartSolve function"
                });
            }

            return StatusCode(202, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = $"Internal server error while starting solver for horizon '{id}' - StartSolve function. Details: {ex.Message}"
            });
        }
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
            return NotFound(new
            {
                error = $"Solver run process not found with runId '{runId}' - GetStatus function"
            });
        }

        return Ok(result);
    }
}