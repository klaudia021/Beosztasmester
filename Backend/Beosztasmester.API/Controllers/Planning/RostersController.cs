using Beosztasmester.Models;
using Beosztasmester.Models.DTOs;
using Beosztasmester.Services;
using Microsoft.AspNetCore.Mvc;

namespace Beosztasmester.Controllers.Planning;

[ApiController]
[Route("api/[controller]")]
public class RostersController : ControllerBase
{
    private readonly IRostersService _rostersService;

    public RostersController(IRostersService rostersService)
    {
        _rostersService = rostersService;
    }

    /// <summary>
    /// Egy horizont összes beosztásverziójának listázása
    /// </summary>
    [HttpGet("horizon/{horizonId:int}")]
    public async Task<ActionResult<IEnumerable<RosterVersionDto>>> GetRostersByHorizon(int horizonId)
    {
        var versions = await _rostersService.GetRostersByHorizonAsync(horizonId);
        return Ok(versions);
    }

    /// <summary>
    /// Egy konkrét beosztásverzió részletei a hozzárendelésekkel együtt (F5)
    /// </summary>
    [HttpGet("{versionId:int}")]
    public async Task<ActionResult<RosterVersionDto>> GetRosterVersion(int versionId)
    {
        var version = await _rostersService.GetRosterVersionByIdAsync(versionId);
        if (version == null) return NotFound(new { error = "not found" });

        return Ok(version);
    }

    /// <summary>
    /// Új beosztásverzió létrehozása a horizont alatt
    /// </summary>
    [HttpPost("horizon/{horizonId:int}")]
    public async Task<ActionResult<RosterVersionDto>> CreateRosterVersion(int horizonId, [FromQuery] string createdBy = "Planner")
    {
        var result = await _rostersService.CreateRosterVersionAsync(horizonId, createdBy);
        return CreatedAtAction(nameof(GetRosterVersion), new { versionId = result.Id }, result);
    }

    /// <summary>
    /// F5: Műszakok kézi módosítása / zárolása / törlése és azonnali újravalidációja
    /// </summary>
    [HttpPatch("{versionId:int}/assignments")]
    public async Task<ActionResult<ValidationResultDto>> PatchAssignments(int versionId, [FromBody] List<PatchAssignmentDto> patchDtos)
    {
        var validationResult = await _rostersService.PatchAssignmentsAsync(versionId, patchDtos);
        return Ok(validationResult);
    }

    /// <summary>
    /// Beosztás kézi újravalidálása (F5)
    /// </summary>
    [HttpPost("{versionId:int}/validate")]
    public async Task<ActionResult<ValidationResultDto>> ValidateRoster(int versionId)
    {
        var result = await _rostersService.ValidateRosterVersionAsync(versionId);
        return Ok(result);
    }

    /// <summary>
    /// F7: Két verzió közötti különbségek lekérdezése (Diff)
    /// </summary>
    [HttpGet("diff")]
    public async Task<ActionResult<RosterDiffDto>> CompareVersions([FromQuery] int versionAId, [FromQuery] int versionBId)
    {
        var diff = await _rostersService.CompareRosterVersionsAsync(versionAId, versionBId);
        if (diff == null) return NotFound(new { error = "Valamelyik verzió nem található!" });

        return Ok(diff);
    }

    /// <summary>
    /// Beosztás publikálása (I2 iCal-ban elérhetővé válik)
    /// </summary>
    [HttpPost("{versionId:int}/publish")]
    public async Task<IActionResult> PublishRoster(int versionId)
    {
        var success = await _rostersService.PublishRosterVersionAsync(versionId);
        if (!success) return NotFound(new { error = "not found" });

        return NoContent();
    }
}