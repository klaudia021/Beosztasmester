using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Beosztasmester.Services;

public interface IRostersService
{
    Task<RosterVersionDto?> GetRosterVersionByIdAsync(int versionId);
    Task<IEnumerable<RosterVersionDto>> GetRostersByHorizonAsync(int horizonId);
    Task<RosterVersionDto> CreateRosterVersionAsync(int horizonId, string createdBy);
    Task<ValidationResultDto> PatchAssignmentsAsync(int versionId, List<PatchAssignmentDto> patchDtos);
    Task<ValidationResultDto> ValidateRosterVersionAsync(int versionId);
    Task<RosterDiffDto?> CompareRosterVersionsAsync(int versionAId, int versionBId);
    Task<bool> PublishRosterVersionAsync(int versionId);
}
public class RostersService: IRostersService
{
    private readonly DataContext _dataContext;

    public RostersService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<RosterVersionDto?> GetRosterVersionByIdAsync(int versionId)
    {
        var version = await _dataContext.RosterVersions
            .Include(v => v.Assignments)
            .FirstOrDefaultAsync(v => v.Id == versionId);

        if (version == null) return null;

        return new RosterVersionDto
        {
            Id = version.Id,
            HorizonId = version.HorizonId,
            VersionNumber = version.VersionNumber,
            Status = version.Status,
            CreatedAt = version.CreatedDate,
            CreatedBy = version.CreatedBy,
            Assignments = version.Assignments.Select(a => new AssignmentDto
            {
                Id = a.Id,
                RosterVersionId = a.RosterVersionId,
                EmployeeId = a.EmployeeId,
                Date = a.Date,
                ShiftTypeId = a.ShiftTypeId,
                IsLocked = a.IsLocked
            }).ToList()
        };
    }

    public async Task<IEnumerable<RosterVersionDto>> GetRostersByHorizonAsync(int horizonId)
    {
        var versions = await _dataContext.RosterVersions
            .Where(v => v.HorizonId == horizonId)
            .OrderByDescending(v => v.VersionNumber)
            .ToListAsync();

        return versions.Select(v => new RosterVersionDto
        {
            Id = v.Id,
            HorizonId = v.HorizonId,
            VersionNumber = v.VersionNumber,
            Status = v.Status,
            CreatedAt = v.CreatedDate,
            CreatedBy = v.CreatedBy
        });
    }

    public async Task<RosterVersionDto> CreateRosterVersionAsync(int horizonId, string createdBy)
    {
        // Kiszámoljuk a következő verziószámot az adott horizonton belül
        var maxVersionNumber = await _dataContext.RosterVersions
            .Where(v => v.HorizonId == horizonId)
            .MaxAsync(v => (int?)v.VersionNumber) ?? 0;

        var newVersion = new RosterVersion
        {
            HorizonId = horizonId,
            VersionNumber = maxVersionNumber + 1,
            Status = "Draft",
            CreatedDate = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        _dataContext.RosterVersions.Add(newVersion);
        await _dataContext.SaveChangesAsync();

        return new RosterVersionDto
        {
            Id = newVersion.Id,
            HorizonId = newVersion.HorizonId,
            VersionNumber = newVersion.VersionNumber,
            Status = newVersion.Status,
            CreatedAt = newVersion.CreatedDate,
            CreatedBy = newVersion.CreatedBy,
            Assignments = new List<AssignmentDto>()
        };
    }

    /// <summary>
    /// F5: Kézi módosítások végrehajtása (zárolás/feloldás, műszak törlés/hozzáadás)
    /// </summary>
    public async Task<ValidationResultDto> PatchAssignmentsAsync(int versionId, List<PatchAssignmentDto> patchDtos)
    {
        var version = await _dataContext.RosterVersions
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
                .FirstOrDefault(a => a.EmployeeId == dto.EmployeeId && a.Date.Date == dateUtc.Date);

            if (dto.ShiftTypeId == null || dto.ShiftTypeId == 0)
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
                    existingAssignment.ShiftTypeId = dto.ShiftTypeId.Value;
                    existingAssignment.IsLocked = dto.IsLocked;
                    existingAssignment.Source = "Manual";
                }
                else
                {
                    // Új hozzárendelés kézzel
                    var newAssignment = new Assignment
                    {
                        RosterVersionId = versionId,
                        EmployeeId = dto.EmployeeId,
                        Date = dateUtc,
                        ShiftTypeId = dto.ShiftTypeId.Value,
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
    public async Task<ValidationResultDto> ValidateRosterVersionAsync(int versionId)
    {
        var result = new ValidationResultDto
        {
            IsValid = true,
            HardViolations = new List<RuleViolationDto>(),
            SoftViolations = new List<RuleViolationDto>(),
            TotalSoftPenalty = 0
        };

        var assignments = await _dataContext.Assignments
            .Where(a => a.RosterVersionId == versionId)
            .ToListAsync();

        // Példa ellenőrzés: Dupla műszak ugyanazon a napon egy dolgozónak
        var duplicateShifts = assignments
            .GroupBy(a => new { a.EmployeeId, a.Date.Date })
            .Where(g => g.Count() > 1);

        foreach (var dup in duplicateShifts)
        {
            result.IsValid = false;
            result.HardViolations.Add(new RuleViolationDto
            {
                RuleTypeCode = "NO_DOUBLE_SHIFT",
                Message = "Egy dolgozónak nem lehet egynél több műszakja ugyanazon a napon!",
                AffectedEmployeeId = dup.Key.EmployeeId,
                AffectedDate = dup.Key.Date
            });
        }

        return result;
    }

    /// <summary>
    /// F7: Két beosztásverzió közötti eltérések (Diff) kiszámítása
    /// </summary>
    public async Task<RosterDiffDto?> CompareRosterVersionsAsync(int versionAId, int versionBId)
    {
        var assignmentsA = await _dataContext.Assignments
            .Where(a => a.RosterVersionId == versionAId)
            .ToListAsync();

        var assignmentsB = await _dataContext.Assignments
            .Where(a => a.RosterVersionId == versionBId)
            .ToListAsync();

        var diff = new RosterDiffDto
        {
            VersionAId = versionAId,
            VersionBId = versionBId,
            Changes = new List<AssignmentChangeDto>()
        };

        var allKeys = assignmentsA.Select(a => new { a.EmployeeId, Date = a.Date.Date })
            .Union(assignmentsB.Select(b => new { b.EmployeeId, Date = b.Date.Date }))
            .Distinct();

        foreach (var key in allKeys)
        {
            var assignA = assignmentsA.FirstOrDefault(a => a.EmployeeId == key.EmployeeId && a.Date.Date == key.Date);
            var assignB = assignmentsB.FirstOrDefault(b => b.EmployeeId == key.EmployeeId && b.Date.Date == key.Date);

            if (assignA == null && assignB != null)
            {
                diff.Changes.Add(new AssignmentChangeDto
                {
                    EmployeeId = key.EmployeeId,
                    Date = key.Date,
                    OldShiftTypeId = null,
                    NewShiftTypeId = assignB.ShiftTypeId,
                    ChangeType = "Added"
                });
            }
            else if (assignA != null && assignB == null)
            {
                diff.Changes.Add(new AssignmentChangeDto
                {
                    EmployeeId = key.EmployeeId,
                    Date = key.Date,
                    OldShiftTypeId = assignA.ShiftTypeId,
                    NewShiftTypeId = null,
                    ChangeType = "Removed"
                });
            }
            else if (assignA != null && assignB != null && assignA.ShiftTypeId != assignB.ShiftTypeId)
            {
                diff.Changes.Add(new AssignmentChangeDto
                {
                    EmployeeId = key.EmployeeId,
                    Date = key.Date,
                    OldShiftTypeId = assignA.ShiftTypeId,
                    NewShiftTypeId = assignB.ShiftTypeId,
                    ChangeType = "Modified"
                });
            }
        }

        return diff;
    }

    public async Task<bool> PublishRosterVersionAsync(int versionId)
    {
        var version = await _dataContext.RosterVersions.FindAsync(versionId);
        if (version == null) return false;

        version.Status = "Published";
        await _dataContext.SaveChangesAsync();
        return true;
    }
}
