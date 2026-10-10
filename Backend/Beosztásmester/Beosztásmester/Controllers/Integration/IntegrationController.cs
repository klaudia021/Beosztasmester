using Beosztasmester.Models;
using Beosztasmester.Models.DTOs;
using Beosztasmester.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Beosztasmester.Controllers.Integration;

[ApiController]
[Route("api/[controller]")]
public class IntegrationController : ControllerBase
{
    private readonly IIntegrationService _integrationService;

    public IntegrationController(IIntegrationService integrationService)
    {
        _integrationService = integrationService;
    }

    /// <summary>
    /// I2: Személyes iCal naptárfolyam (.ics) letöltése token alapján (Naptár-előfizetéshez)
    /// </summary>
    [HttpGet("ical/{token}.ics")]
    [Produces("text/calendar")]
    public async Task<IActionResult> GetICalFeed(string token)
    {
        var calendarContent = await _integrationService.GenerateICalFeedAsync(token);
        if (calendarContent == null) return NotFound();

        return File(Encoding.UTF8.GetBytes(calendarContent), "text/calendar", "roster.ics");
    }

    /// <summary>
    /// I3: Egy beosztásverzió exportálása CSV fájlként
    /// </summary>
    [HttpGet("rosters/{versionId:guid}/export/csv")]
    public async Task<IActionResult> ExportRosterCsv(Guid versionId)
    {
        var csvContent = await _integrationService.ExportRosterToCsvAsync(versionId);
        if (csvContent == null) return NotFound();

        var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csvContent)).ToArray();
        return File(bytes, "text/csv", $"roster_export_{versionId}.csv");
    }

    /// <summary>
    /// I1: Dolgozói törzsadat CSV fájl feltöltése és ellenőrző előnézete
    /// </summary>
    [HttpPost("import/employees/preview")]
    public async Task<ActionResult<EmployeeImportPreviewDto>> PreviewEmployeeImport(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "Kérlek tölts fel egy érvényes CSV fájlt!" });
        }

        var preview = await _integrationService.PreviewEmployeeImportAsync(file);
        return Ok(preview);
    }
}