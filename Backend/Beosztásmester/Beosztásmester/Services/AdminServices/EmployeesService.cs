using Microsoft.EntityFrameworkCore;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Beosztasmester.Model;

namespace Beosztasmester.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid? departmentId, string? status, string? search);
    Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id);
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<bool> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto);
    Task<bool> DeleteEmployeeAsync(Guid id);
}

public class EmployeesService : IEmployeeService
{
   
    private readonly DataContext _dataContext;

    public EmployeesService(DataContext c)
    {
        _dataContext = c; 
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid? departmentId, string? status, string? search)
    {
        var query = _dataContext.Employees
            .Include(e => e.Employee_Competencies)
            .AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(e => e.Department_Id == departmentId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.Status.ToLower() == status.ToLower());

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e => e.Name.Contains(search));

        return await query.Select(e => new EmployeeDto
        {
            Id = e.Id,
            Department_Id = e.Department_Id,
            Name = e.Name,
            Status = e.Status,
            Contract_Hours_Per_Week = e.Contract_Hours_Per_Week,
            Entry_Date = e.Entry_Date,
            Exit_Date = e.Exit_Date,
            SkillIds = e.Employee_Competencies.Select(s => s.Competency_Id).ToList()
        }).ToListAsync();
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _dataContext.Employees
            .Include(e => e.Employee_Competencies)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null) return null;

        return new EmployeeDto
        {
            Id = employee.Id,
            Department_Id = employee.Department_Id,
            Name = employee.Name,
            Status = employee.Status,
            Contract_Hours_Per_Week = employee.Contract_Hours_Per_Week,
            Entry_Date = employee.Entry_Date,
            Exit_Date = employee.Exit_Date,
            SkillIds = employee.Employee_Competencies.Select(s => s.Competency_Id).ToList()
        };
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Department_Id = dto.Department_Id,
            Name = dto.Name,
            Contract_Hours_Per_Week = dto.Contract_Hours_Per_Week,
            Entry_Date = dto.Entry_Date,
            Status = "Active"
        };

        if (dto.SkillIds != null && dto.SkillIds.Any())
        {
            foreach (var skillId in dto.SkillIds)
            {
                employee.Employee_Competencies.Add(new Employee_Competency
                {
                    Employee_Id = employee.Id,
                    Competency_Id = skillId,
                    Valid_From = DateTime.UtcNow
                });
            }
        }

        _dataContext.Employees.Add(employee);
        await _dataContext.SaveChangesAsync();

        return new EmployeeDto
        {
            Id = employee.Id,
            Department_Id = employee.Department_Id,
            Name = employee.Name,
            Status = employee.Status,
            Contract_Hours_Per_Week = employee.Contract_Hours_Per_Week,
            Entry_Date = employee.Entry_Date,
            SkillIds = dto.SkillIds ?? new List<Guid>()
        };
    }

    public async Task<bool> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto)
    {
        var employee = await _dataContext.Employees
            .Include(e => e.Employee_Competencies)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null) return false;

        employee.Name = dto.Name;
        employee.Status = dto.Status;
        employee.Contract_Hours_Per_Week = dto.Contract_Hours_Per_Week;
        employee.Exit_Date = dto.Exit_Date;

        _dataContext.Employee_Competencies.RemoveRange(employee.Employee_Competencies);
        if (dto.SkillIds != null)
        {
            foreach (var skillId in dto.SkillIds)
            {
                employee.Employee_Competencies.Add(new Employee_Competency
                {
                    Employee_Id = employee.Id,
                    Competency_Id = skillId,
                    Valid_From = DateTime.UtcNow
                });
            }
        }

        await _dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteEmployeeAsync(Guid id)
    {
        var employee = await _dataContext.Employees.FindAsync(id);
        if (employee == null) return false;

        employee.Status = "Inactive";
        employee.Exit_Date = DateTime.UtcNow;

        await _dataContext.SaveChangesAsync();
        return true;
    }


}
