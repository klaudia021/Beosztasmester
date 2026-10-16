using Microsoft.EntityFrameworkCore;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Beosztasmester.Models;

namespace Beosztasmester.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(int? departmentId, string? status, string? search);
    Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);
    Task<bool> DeleteEmployeeAsync(int id);
}

public class EmployeeService : IEmployeeService
{
    private readonly DataContext _dataContext;

    public EmployeeService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(int? departmentId, string? status, string? search)
    {
        var query = _dataContext.Employees
            .Include(e => e.EmployeeCompetencies)
            .AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(e => e.DepartmentId == departmentId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.Status.ToLower() == status.ToLower());

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e => e.Name.Contains(search));

        return await query.Select(e => new EmployeeDto
        {
            Id = e.Id,
            DepartmentId = e.DepartmentId,
            Name = e.Name,
            Status = e.Status,
            ContractHoursPerWeek = e.ContractHoursPerWeek,
            EntryDate = e.EntryDate,
            ExitDate = e.ExitDate,
            SkillIds = e.EmployeeCompetencies.Select(s => s.CompetencyId).ToList()
        }).ToListAsync();
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _dataContext.Employees
            .Include(e => e.EmployeeCompetencies)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null) return null;

        return new EmployeeDto
        {
            Id = employee.Id,
            DepartmentId = employee.DepartmentId,
            Name = employee.Name,
            Status = employee.Status,
            ContractHoursPerWeek = employee.ContractHoursPerWeek,
            EntryDate = employee.EntryDate,
            ExitDate = employee.ExitDate,
            SkillIds = employee.EmployeeCompetencies.Select(s => s.CompetencyId).ToList()
        };
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            DepartmentId = dto.DepartmentId,
            Name = dto.Name,
            ContractHoursPerWeek = dto.ContractHoursPerWeek,
            EntryDate = dto.EntryDate,
            Status = "Active"
        };

        if (dto.SkillIds != null && dto.SkillIds.Any())
        {
            foreach (var skillId in dto.SkillIds)
            {
                employee.EmployeeCompetencies.Add(new EmployeeCompetency
                {
                    EmployeeId = employee.Id,
                    CompetencyId = skillId,
                    ValidFrom = DateTime.UtcNow
                });
            }
        }

        _dataContext.Employees.Add(employee);
        await _dataContext.SaveChangesAsync();

        return new EmployeeDto
        {
            Id = employee.Id,
            DepartmentId = employee.DepartmentId,
            Name = employee.Name,
            Status = employee.Status,
            ContractHoursPerWeek = employee.ContractHoursPerWeek,
            EntryDate = employee.EntryDate,
            SkillIds = dto.SkillIds ?? new List<int>()
        };
    }

    public async Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await _dataContext.Employees
            .Include(e => e.EmployeeCompetencies)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null) return false;

        employee.Name = dto.Name;
        employee.Status = dto.Status;
        employee.ContractHoursPerWeek = dto.ContractHoursPerWeek;
        employee.ExitDate = dto.ExitDate;

        _dataContext.EmployeeCompetencies.RemoveRange(employee.EmployeeCompetencies);
        if (dto.SkillIds != null)
        {
            foreach (var skillId in dto.SkillIds)
            {
                employee.EmployeeCompetencies.Add(new EmployeeCompetency
                {
                    EmployeeId = employee.Id,
                    CompetencyId = skillId,
                    ValidFrom = DateTime.UtcNow
                });
            }
        }

        await _dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _dataContext.Employees.FindAsync(id);
        if (employee == null) return false;

        employee.Status = "Inactive";
        employee.ExitDate = DateTime.UtcNow;

        await _dataContext.SaveChangesAsync();
        return true;
    }


}
