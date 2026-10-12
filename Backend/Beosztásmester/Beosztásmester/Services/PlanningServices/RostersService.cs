using Beosztasmester.Model;
using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Beosztasmester.Services;

public interface IRostersService
{
    Task<RosterVersionDto?> GetRosterVersionByIdAsync(Guid versionId);
    Task<IEnumerable<RosterVersionDto>> GetRostersByHorizonAsync(Guid horizonId);
    Task<RosterVersionDto> CreateRosterVersionAsync(Guid horizonId, string createdBy);
    Task<ValidationResultDto> PatchAssignmentsAsync(Guid versionId, List<PatchAssignmentDto> patchDtos);
    Task<ValidationResultDto> ValidateRosterVersionAsync(Guid versionId);
    Task<RosterDiffDto?> CompareRosterVersionsAsync(Guid versionAId, Guid versionBId);
    Task<bool> PublishRosterVersionAsync(Guid versionId);
}
public class RostersService: IRostersService
{
    private readonly DataContext _dataContext;

    public RostersService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<RosterVersionDto?> GetRosterVersionByIdAsync(Guid versionId)
    {
        var version = await _dataContext.Roster_Versions
            .Include(v => v.Assignments)
            .FirstOrDefaultAsync(v => v.Id == versionId);

        if (version == null) return null;

        return new RosterVersionDto
        {
            Id = version.Id,
            Horizon_Id = version.Horizon_Id,
            Version_Number = version.Version_Number,
            Status = version.Status,
            Created_At = version.Created_Date,
            Created_By = version.Created_By,
            Assignments = version.Assignments.Select(a => new AssignmentDto
            {
                Id = a.Id,
                Roster_Version_Id = a.Roster_Version_Id,
                Employee_Id = a.Employee_Id,
                Date = a.Date,
                Shift_Type_Id = a.Shift_Type_Id,
                IsLocked = a.IsLocked
            }).ToList()
        };
    }

    public async Task<IEnumerable<RosterVersionDto>> GetRostersByHorizonAsync(Guid horizonId)
    {
        var versions = await _dataContext.Roster_Versions
            .Where(v => v.Horizon_Id == horizonId)
            .OrderByDescending(v => v.Version_Number)
            .ToListAsync();

        return versions.Select(v => new RosterVersionDto
        {
            Id = v.Id,
            Horizon_Id = v.Horizon_Id,
            Version_Number = v.Version_Number,
            Status = v.Status,
            Created_At = v.Created_Date,
            Created_By = v.Created_By
        });
    }

    public async Task<RosterVersionDto> CreateRosterVersionAsync(Guid horizonId, string createdBy)
    {
        // Kiszámoljuk a következő verziószámot az adott horizonton belül
        var maxVersionNumber = await _dataContext.Roster_Versions
            .Where(v => v.Horizon_Id == horizonId)
            .MaxAsync(v => (int?)v.Version_Number) ?? 0;

        var newVersion = new Roster_Version
        {
            Id = Guid.NewGuid(),
            Horizon_Id = horizonId,
            Version_Number = maxVersionNumber + 1,
            Status = "Draft",
            Created_Date = DateTime.UtcNow,
            Created_By = createdBy
        };

        _dataContext.Roster_Versions.Add(newVersion);
        await _dataContext.SaveChangesAsync();

        return new RosterVersionDto
        {
            Id = newVersion.Id,
            Horizon_Id = newVersion.Horizon_Id,
            Version_Number = newVersion.Version_Number,
            Status = newVersion.Status,
            Created_At = newVersion.Created_Date,
            Created_By = newVersion.Created_By,
            Assignments = new List<AssignmentDto>()
        };
    }

    /// <summary>
    /// F5: Kézi módosítások végrehajtása (zárolás/feloldás, műszak törlés/hozzáadás)
    /// </summary>
    public async Task<ValidationResultDto> PatchAssignmentsAsync(Guid versionId, List<PatchAssignmentDto> patchDtos)
    {
        var version = await _dataContext.Roster_Versions
            .Include(v => v.Assignments)
            .FirstOrDefaultAsync(v => v.Id == versionId);

        if (version == null)
        {
            return new ValidationResultDto { IsValid = false };
        }

        foreach (var dto in patchDtos)
        {
            var dateUtc = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc);
            var existingAssignment = version.Assignments
                .FirstOrDefault(a => a.Employee_Id == dto.Employee_Id && a.Date.Date == dateUtc.Date);

            if (dto.Shift_Type_Id == null || dto.Shift_Type_Id == Guid.Empty)
            {
                // Műszak törlése
                if (existingAssignment != null)
                {
                    _dataContext.Assignments.Remove(existingAssignment);
                }
            }
            else
            {
                if (existingAssignment != null)
                {
                    // Módosítás
                    existingAssignment.Shift_Type_Id = dto.Shift_Type_Id.Value;
                    existingAssignment.IsLocked = dto.IsLocked;
                    existingAssignment.Source = "Manual";
                }
                else
                {
                    // Új hozzárendelés kézzel
                    var newAssignment = new Assignment
                    {
                        Id = Guid.NewGuid(),
                        Roster_Version_Id = versionId,
                        Employee_Id = dto.Employee_Id,
                        Date = dateUtc,
                        Shift_Type_Id = dto.Shift_Type_Id.Value,
                        IsLocked = dto.IsLocked,
                        Source = "Manual"
                    };
                    _dataContext.Assignments.Add(newAssignment);
                }
            }
        }

        await _dataContext.SaveChangesAsync();

        // Módosítás után futtatjuk az azonnali inkrementális szabály-ellenőrzést
        return await ValidateRosterVersionAsync(versionId);
    }

    /// <summary>
    /// F5: Beosztás érvényességének ellenőrzése (Hard és Soft szabályok vizsgálata)
    /// </summary>
    public async Task<ValidationResultDto> ValidateRosterVersionAsync(Guid versionId)
    {
        var result = new ValidationResultDto
        {
            IsValid = true,
            HardViolations = new List<RuleViolationDto>(),
            SoftViolations = new List<RuleViolationDto>(),
            TotalSoftPenalty = 0
        };

        var assignments = await _dataContext.Assignments
            .Where(a => a.Roster_Version_Id == versionId)
            .ToListAsync();

        // Példa ellenőrzés: Dupla műszak ugyanazon a napon egy dolgozónak
        var duplicateShifts = assignments
            .GroupBy(a => new { a.Employee_Id, a.Date.Date })
            .Where(g => g.Count() > 1);

        foreach (var dup in duplicateShifts)
        {
            result.IsValid = false;
            result.HardViolations.Add(new RuleViolationDto
            {
                RuleTypeCode = "NO_DOUBLE_SHIFT",
                Message = "Egy dolgozónak nem lehet egynél több műszakja ugyanazon a napon!",
                AffectedEmployeeId = dup.Key.Employee_Id,
                AffectedDate = dup.Key.Date
            });
        }

        return result;
    }

    /// <summary>
    /// F7: Két beosztásverzió közötti eltérések (Diff) kiszámítása
    /// </summary>
    public async Task<RosterDiffDto?> CompareRosterVersionsAsync(Guid versionAId, Guid versionBId)
    {
        var assignmentsA = await _dataContext.Assignments
            .Where(a => a.Roster_Version_Id == versionAId)
            .ToListAsync();

        var assignmentsB = await _dataContext.Assignments
            .Where(a => a.Roster_Version_Id == versionBId)
            .ToListAsync();

        var diff = new RosterDiffDto
        {
            Version_A_Id = versionAId,
            Version_B_Id = versionBId,
            Changes = new List<AssignmentChangeDto>()
        };

        var allKeys = assignmentsA.Select(a => new { a.Employee_Id, Date = a.Date.Date })
            .Union(assignmentsB.Select(b => new { b.Employee_Id, Date = b.Date.Date }))
            .Distinct();

        foreach (var key in allKeys)
        {
            var assignA = assignmentsA.FirstOrDefault(a => a.Employee_Id == key.Employee_Id && a.Date.Date == key.Date);
            var assignB = assignmentsB.FirstOrDefault(b => b.Employee_Id == key.Employee_Id && b.Date.Date == key.Date);

            if (assignA == null && assignB != null)
            {
                diff.Changes.Add(new AssignmentChangeDto
                {
                    Employee_Id = key.Employee_Id,
                    Date = key.Date,
                    Old_Shift_Type_Id = null,
                    New_Shift_Type_Id = assignB.Shift_Type_Id,
                    Change_Type = "Added"
                });
            }
            else if (assignA != null && assignB == null)
            {
                diff.Changes.Add(new AssignmentChangeDto
                {
                    Employee_Id = key.Employee_Id,
                    Date = key.Date,
                    Old_Shift_Type_Id = assignA.Shift_Type_Id,
                    New_Shift_Type_Id = null,
                    Change_Type = "Removed"
                });
            }
            else if (assignA != null && assignB != null && assignA.Shift_Type_Id != assignB.Shift_Type_Id)
            {
                diff.Changes.Add(new AssignmentChangeDto
                {
                    Employee_Id = key.Employee_Id,
                    Date = key.Date,
                    Old_Shift_Type_Id = assignA.Shift_Type_Id,
                    New_Shift_Type_Id = assignB.Shift_Type_Id,
                    Change_Type = "Modified"
                });
            }
        }

        return diff;
    }

    public async Task<bool> PublishRosterVersionAsync(Guid versionId)
    {
        var version = await _dataContext.Roster_Versions.FindAsync(versionId);
        if (version == null) return false;

        version.Status = "Published";
        await _dataContext.SaveChangesAsync();
        return true;
    }
}

