using Beosztasmester.Models;
using Beosztasmester.Models.DTOs;
using Beosztasmester.Services;
using Microsoft.AspNetCore.Mvc;

namespace Beosztasmester.Controllers.Planning;

[ApiController]
[Route("api/[controller]")]
public class DemandsController : ControllerBase
{
    private readonly IDemandsService _demandsService;

    public DemandsController(IDemandsService demandsService)
    {
        _demandsService = demandsService;
    }

    /// <summary>
    /// Kapacitásigények lekérdezése szervezeti egységre és dátumtartományra
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DemandDto>>> GetDemands(
        [FromQuery] Guid departmentId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest(new { message = "DepartmentId megadása kötelező!" });
        }

        var demands = await _demandsService.GetDemandsAsync(departmentId, fromDate, toDate);
        return Ok(demands);
    }

    /// <summary>
    /// Egy konkrét kapacitásigény lekérdezése ID alapján
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DemandDto>> GetDemand(Guid id)
    {
        var demand = await _demandsService.GetDemandByIdAsync(id);
        if (demand == null) return NotFound(new { error = "not found" });

        return Ok(demand);
    }

    /// <summary>
    /// Új kapacitásigény rögzítése
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<DemandDto>> CreateDemand([FromBody] CreateDemandDto dto)
    {
        var result = await _demandsService.CreateDemandAsync(dto);
        return CreatedAtAction(nameof(GetDemand), new { id = result.Id }, result);
    }

    /// <summary>
    /// Kapacitásigények tömeges mentése / frissítése (mátrix felülethez)
    /// </summary>
    [HttpPost("bulk")]
    public async Task<ActionResult<IEnumerable<DemandDto>>> BulkUpsertDemands(
        [FromQuery] Guid departmentId,
        [FromBody] List<CreateDemandDto> dtos)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest(new { message = "DepartmentId megadása kötelező!" });
        }

        var results = await _demandsService.BulkUpsertDemandsAsync(departmentId, dtos);
        return Ok(results);
    }

    /// <summary>
    /// Kapacitásigény törlése
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDemand(Guid id)
    {
        var deleted = await _demandsService.DeleteDemandAsync(id);
        if (!deleted) return NotFound(new { error = "not found" });

        return NoContent();
    }
}