using Beosztasmester.DTOs;
using Beosztasmester.Models.DTOs;
using Beosztasmester.Services;
using Microsoft.AspNetCore.Mvc;

namespace Beosztasmester.Controllers.Employee;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly IRequestsService _requestsService;

    public RequestsController(IRequestsService requestsService)
    {
        _requestsService = requestsService;
    }

    /// <summary>
    /// Kérések listázása (szűrhető dolgozóra vagy státuszra)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeRequestDto>>> GetRequests(
        [FromQuery] Guid? employeeId,
        [FromQuery] string? status)
    {
        var requests = await _requestsService.GetRequestsAsync(employeeId, status);
        return Ok(requests);
    }

    /// <summary>
    /// Konkrét kérés lekérdezése ID alapján
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeRequestDto>> GetRequest(Guid id)
    {
        var request = await _requestsService.GetRequestByIdAsync(id);
        if (request == null) return NotFound();

        return Ok(request);
    }

    /// <summary>
    /// Új kérés benyújtása dolgozó által
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EmployeeRequestDto>> CreateRequest([FromBody] CreateEmployeeRequestDto dto)
    {
        var result = await _requestsService.CreateRequestAsync(dto);
        return CreatedAtAction(nameof(GetRequest), new { id = result.Id }, result);
    }

    /// <summary>
    /// Kérés állapotának módosítása tervező által (Approved / Rejected)
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateRequestStatus(Guid id, [FromBody] UpdateRequestStatusDto dto)
    {
        var updated = await _requestsService.UpdateRequestStatusAsync(id, dto);
        if (!updated) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Kérés visszavonása / törlése
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRequest(Guid id)
    {
        var deleted = await _requestsService.DeleteRequestAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}