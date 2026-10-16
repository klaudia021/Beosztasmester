using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Beosztasmester.Services;

public interface IMasterDataService
{
    // Departments
    Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync();
    Task<DepartmentDto?> GetDepartmentByIdAsync(int id);
    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto);

    // Skills
    Task<IEnumerable<SkillDto>> GetSkillsAsync();
    Task<SkillDto?> GetSkillByIdAsync(int id);
    Task<SkillDto> CreateSkillAsync(CreateSkillDto dto);

    // ShiftTypes
    Task<IEnumerable<ShiftTypeDto>> GetShiftTypesAsync(int? departmentId);
    Task<ShiftTypeDto?> GetShiftTypeByIdAsync(int id);
    Task<ShiftTypeDto> CreateShiftTypeAsync(CreateShiftTypeDto dto);
}

public class MasterDataService : IMasterDataService
{
    private readonly DataContext _dataContext;

    public MasterDataService(DataContext c)
    {
        _dataContext = c;
    }
    public async Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync()
    {
        return await _dataContext.Department
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                ParentId = d.ParentId
            }).ToListAsync();
    }
    public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
    {
        var dept = await _dataContext.Department.FindAsync(id);
        if (dept == null) return null;

        return new DepartmentDto
        {
            Id = dept.Id,
            Name = dept.Name,
            ParentId = dept.ParentId
        };
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
    {
        var department = new Department
        {
            Name = dto.Name,
            ParentId = dto.ParentId
        };

        _dataContext.Department.Add(department);
        await _dataContext.SaveChangesAsync();

        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            ParentId = department.ParentId
        };
    }

    // --- Skills ---
    public async Task<IEnumerable<SkillDto>> GetSkillsAsync()
    {
        return await _dataContext.Competencies
            .Select(s => new SkillDto
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name
            }).ToListAsync();
    }

    public async Task<SkillDto?> GetSkillByIdAsync(int id)
    {
        var skill = await _dataContext.Competencies.FindAsync(id);
        if (skill == null) return null;

        return new SkillDto
        {
            Id = skill.Id,
            Code = skill.Code,
            Name = skill.Name
        };
    }

    public async Task<SkillDto> CreateSkillAsync(CreateSkillDto dto)
    {
        var skill = new Competency
        {
            Code = dto.Code,
            Name = dto.Name
        };

        _dataContext.Competencies.Add(skill);
        await _dataContext.SaveChangesAsync();

        return new SkillDto
        {
            Id = skill.Id,
            Code = skill.Code,
            Name = skill.Name
        };
    }

    // --- ShiftTypes ---
    public async Task<IEnumerable<ShiftTypeDto>> GetShiftTypesAsync(int? departmentId)
    {
        var query = _dataContext.ShiftTypes.AsQueryable();

        if (departmentId.HasValue)
        {
            query = query.Where(st => st.DepartmentId == departmentId.Value);
        }

        return await query.Select(st => new ShiftTypeDto
        {
            Id = st.Id,
            DepartmentId = st.DepartmentId,
            Code = st.Code,
            StartTime = st.StartTime.ToString(@"hh\:mm"),
            EndTime = st.EndTime.ToString(@"hh\:mm"),
            DurationMinutes = st.DurationMinutes,
            IsNight = st.IsNightShift
        }).ToListAsync();
    }

    public async Task<ShiftTypeDto?> GetShiftTypeByIdAsync(int id)
    {
        var st = await _dataContext.ShiftTypes.FindAsync(id);
        if (st == null) return null;

        return new ShiftTypeDto
        {
            Id = st.Id,
            DepartmentId = st.DepartmentId,
            Code = st.Code,
            StartTime = st.StartTime.ToString(@"hh\:mm"),
            EndTime = st.EndTime.ToString(@"hh\:mm"),
            DurationMinutes = st.DurationMinutes,
            IsNight = st.IsNightShift
        };
    }

    public async Task<ShiftTypeDto> CreateShiftTypeAsync(CreateShiftTypeDto dto)
    {
        var shiftType = new ShiftType
        {
            DepartmentId = dto.DepartmentId,
            Code = dto.Code,
            StartTime = TimeSpan.Parse(dto.StartTime),
            EndTime = TimeSpan.Parse(dto.EndTime),
            DurationMinutes = dto.DurationMinutes,
            IsNightShift = dto.IsNight
        };

        _dataContext.ShiftTypes.Add(shiftType);
        await _dataContext.SaveChangesAsync();

        return new ShiftTypeDto
        {
            Id = shiftType.Id,
            DepartmentId = shiftType.DepartmentId,
            Code = shiftType.Code,
            StartTime = shiftType.StartTime.ToString(@"hh\:mm"),
            EndTime = shiftType.EndTime.ToString(@"hh\:mm"),
            DurationMinutes = shiftType.DurationMinutes,
            IsNight = shiftType.IsNightShift
        };
    }

}
