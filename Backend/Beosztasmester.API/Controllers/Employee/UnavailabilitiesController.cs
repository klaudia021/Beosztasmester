
using Microsoft.AspNetCore.Mvc;
using Beosztasmester.Models.DTOs;
using Beosztasmester.Services;

namespace Beosztasmester.Controllers.Employee;

[ApiController]
[Route("api/[controller]")]
public class UnavailabilitiesController : ControllerBase
{
    private readonly IUnavailabilitiesService _unavailabilitiesService;

    public UnavailabilitiesController(IUnavailabilitiesService unavailabilitiesService)
    {
        _unavailabilitiesService = unavailabilitiesService;
    }

    /// <summary>
    /// Elérhetetlenségek listázása (szűrhető dolgozóra és dátumtartományra)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UnavailabilityDto>>> GetUnavailabilities(
        [FromQuery] Guid? employeeId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var results = await _unavailabilitiesService.GetUnavailabilitiesAsync(employeeId, fromDate, toDate);
        return Ok(results);
    }

    /// <summary>
    /// Konkrét elérhetetlenség lekérdezése ID alapján
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UnavailabilityDto>> GetUnavailability(Guid id)
    {
        var result = await _unavailabilitiesService.GetUnavailabilityByIdAsync(id);
        if (result == null) return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Új elérhetetlenség (szabadság, betegség stb.) rögzítése
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UnavailabilityDto>> CreateUnavailability([FromBody] CreateUnavailabilityDto dto)
    {
        if (dto.StartDate > dto.EndDate)
        {
            return BadRequest(new { message = "A kezdő dátum nem lehet későbbi a záró dátumnál!" });
        }

        var result = await _unavailabilitiesService.CreateUnavailabilityAsync(dto);
        return CreatedAtAction(nameof(GetUnavailability), new { id = result.Id }, result);
    }

    /// <summary>
    /// Elérhetetlenség törlése
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUnavailability(Guid id)
    {
        var deleted = await _unavailabilitiesService.DeleteUnavailabilityAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
