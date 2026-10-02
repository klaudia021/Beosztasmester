using Beosztasmester.Model;
using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Beosztasmester.Services;

public interface IMasterDataService
{
    // Departments
    Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync();
    Task<DepartmentDto?> GetDepartmentByIdAsync(Guid id);
    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto);

    // Skills
    Task<IEnumerable<SkillDto>> GetSkillsAsync();
    Task<SkillDto?> GetSkillByIdAsync(Guid id);
    Task<SkillDto> CreateSkillAsync(CreateSkillDto dto);

    // ShiftTypes
    Task<IEnumerable<ShiftTypeDto>> GetShiftTypesAsync(Guid? departmentId);
    Task<ShiftTypeDto?> GetShiftTypeByIdAsync(Guid id);
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
                ParentId = d.Parent_Id
            }).ToListAsync();
    }
    public async Task<DepartmentDto?> GetDepartmentByIdAsync(Guid id)
    {
        var dept = await _dataContext.Department.FindAsync(id);
        if (dept == null) return null;

        return new DepartmentDto
        {
            Id = dept.Id,
            Name = dept.Name,
            ParentId = dept.Parent_Id
        };
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
    {
        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Parent_Id = dto.ParentId
        };

        _dataContext.Department.Add(department);
        await _dataContext.SaveChangesAsync();

        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            ParentId = department.Parent_Id
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

    public async Task<SkillDto?> GetSkillByIdAsync(Guid id)
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
            Id = Guid.NewGuid(),
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
    public async Task<IEnumerable<ShiftTypeDto>> GetShiftTypesAsync(Guid? departmentId)
    {
        var query = _dataContext.Shift_Types.AsQueryable();

        if (departmentId.HasValue)
        {
            query = query.Where(st => st.Department_Id == departmentId.Value);
        }

        return await query.Select(st => new ShiftTypeDto
        {
            Id = st.Id,
            DepartmentId = st.Department_Id,
            Code = st.Code,
            StartTime = st.Start_Time.ToString(@"hh\:mm"),
            EndTime = st.End_Time.ToString(@"hh\:mm"),
            DurationMinutes = st.Duration_Minutes,
            IsNight = st.Is_Night_Shift
        }).ToListAsync();
    }

    public async Task<ShiftTypeDto?> GetShiftTypeByIdAsync(Guid id)
    {
        var st = await _dataContext.Shift_Types.FindAsync(id);
        if (st == null) return null;

        return new ShiftTypeDto
        {
            Id = st.Id,
            DepartmentId = st.Department_Id,
            Code = st.Code,
            StartTime = st.Start_Time.ToString(@"hh\:mm"),
            EndTime = st.End_Time.ToString(@"hh\:mm"),
            DurationMinutes = st.Duration_Minutes,
            IsNight = st.Is_Night_Shift
        };
    }

    public async Task<ShiftTypeDto> CreateShiftTypeAsync(CreateShiftTypeDto dto)
    {
        var shiftType = new Shift_Type
        {
            Id = Guid.NewGuid(),
            Department_Id = dto.DepartmentId,
            Code = dto.Code,
            Start_Time = TimeSpan.Parse(dto.StartTime),
            End_Time = TimeSpan.Parse(dto.EndTime),
            Duration_Minutes = dto.DurationMinutes,
            Is_Night_Shift = dto.IsNight
        };

        _dataContext.Shift_Types.Add(shiftType);
        await _dataContext.SaveChangesAsync();

        return new ShiftTypeDto
        {
            Id = shiftType.Id,
            DepartmentId = shiftType.Department_Id,
            Code = shiftType.Code,
            StartTime = shiftType.Start_Time.ToString(@"hh\:mm"),
            EndTime = shiftType.End_Time.ToString(@"hh\:mm"),
            DurationMinutes = shiftType.Duration_Minutes,
            IsNight = shiftType.Is_Night_Shift
        };
    }

}
