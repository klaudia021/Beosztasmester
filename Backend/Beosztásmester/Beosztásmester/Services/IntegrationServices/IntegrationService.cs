using Beosztasmester.Model;
using Beosztasmester.Models;
using Beosztasmester.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Beosztasmester.Services;

public interface IIntegrationService
{
    // I2: iCal Export (.ics szövégként)
    Task<string?> GenerateICalFeedAsync(string userToken);

    // I3: CSV Export (Roster matrix formátumban)
    Task<string?> ExportRosterToCsvAsync(Guid versionId);

    // I1: Dolgozói import előnézet és validáció
    Task<EmployeeImportPreviewDto> PreviewEmployeeImportAsync(IFormFile file);
}
public class IntegrationService : IIntegrationService
{
    private readonly DataContext _dataContext;

    public IntegrationService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// I2: iCal naptárfolyam (.ics) generálása a publikált műszakokról
    /// </summary>
    public async Task<string?> GenerateICalFeedAsync(string userToken)
    {
        // Keressük meg a dolgozót a tokenje alapján (példa logika)
        var employee = await _dataContext.Employees
            .FirstOrDefaultAsync(e => e.Id.ToString().StartsWith(userToken) || e.Status == "Active");

        if (employee == null) return null;

        // Csak a PUBLIKÁLT beosztásverziók hozzárendeléseit kérjük le
        var assignments = await _dataContext.Assignments
            .Include(a => a.Roster_Version)
            .Include(a => a.Shift_Type)
            .Where(a => a.Employee_Id == employee.Id && a.Roster_Version.Status == "Published")
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("PRODID:-//Beosztasmester//Muszakbeosztas//HU");
        sb.AppendLine("CALSCALE:GREGORIAN");
        sb.AppendLine($"X-WR-CALNAME:Beosztás - {employee.Name}");

        foreach (var assignment in assignments)
        {
            var dateStr = assignment.Date.ToString("yyyyMMdd");
            var startTime = assignment.Shift_Type?.Start_Time.ToString(@"hhmmss") ?? "080000";
            var endTime = assignment.Shift_Type?.End_Time.ToString(@"hhmmss") ?? "160000";

            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"UID:{assignment.Id}@beosztasmester");
            sb.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
            sb.AppendLine($"DTSTART:{dateStr}T{startTime}");
            sb.AppendLine($"DTEND:{dateStr}T{endTime}");
            sb.AppendLine($"SUMMARY:Műszak: {assignment.Shift_Type?.Code ?? "Beosztás"}");
            sb.AppendLine($"DESCRIPTION:Beosztásmester műszak hozzárendelés");
            sb.AppendLine("END:VEVENT");
        }

        sb.AppendLine("END:VCALENDAR");
        return sb.ToString();
    }

    /// <summary>
    /// I3: Beosztás exportálása CSV mátrixként (Dolgozó x Napok)
    /// </summary>
    public async Task<string?> ExportRosterToCsvAsync(Guid versionId)
    {
        var version = await _dataContext.Roster_Versions
            .FirstOrDefaultAsync(v => v.Id == versionId);

        if (version == null) return null;

        var assignments = await _dataContext.Assignments
            .Include(a => a.Employee)
            .Include(a => a.Shift_Type)
            .Where(a => a.Roster_Version_Id == versionId)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("Dolgozó Neve;Dátum;Műszak Kód;Műszak Kezdete;Műszak Vége");

        foreach (var a in assignments)
        {
            sb.AppendLine($"{a.Employee?.Name};{a.Date:yyyy-MM-dd};{a.Shift_Type?.Code};{a.Shift_Type?.Start_Time};{a.Shift_Type?.End_Time}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// I1: CSV/REST import előnézet és ellenőrzés
    /// </summary>
    public async Task<EmployeeImportPreviewDto> PreviewEmployeeImportAsync(IFormFile file)
    {
        var result = new EmployeeImportPreviewDto();
        var rows = new List<EmployeeImportRowDto>();

        if (file == null || file.Length == 0)
            return result;

        using var reader = new StreamReader(file.OpenReadStream());
        int rowNum = 0;

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            rowNum++;

            if (rowNum == 1) continue; // Fejléc átugrása

            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(';');
            var rowDto = new EmployeeImportRowDto
            {
                RowNumber = rowNum,
                Name = parts.Length > 0 ? parts[0] : "",
                ContractHours = parts.Length > 1 && int.TryParse(parts[1], out var h) ? h : 0,
                SkillsCsv = parts.Length > 2 ? parts[2] : "",
                IsValid = true
            };

            if (string.IsNullOrWhiteSpace(rowDto.Name))
            {
                rowDto.IsValid = false;
                rowDto.ValidationErrors.Add("A név mező nem lehet üres!");
            }

            if (rowDto.ContractHours <= 0)
            {
                rowDto.IsValid = false;
                rowDto.ValidationErrors.Add("A heti óraszámnak pozitívnak kell lennie!");
            }

            rows.Add(rowDto);
        }

        result.TotalRows = rows.Count;
        result.ValidRowsCount = rows.Count(r => r.IsValid);
        result.InvalidRowsCount = rows.Count(r => !r.IsValid);
        result.Rows = rows;

        return result;
    }
}
