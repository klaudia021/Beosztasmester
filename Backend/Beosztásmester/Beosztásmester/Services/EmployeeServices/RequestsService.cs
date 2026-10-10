using Beosztasmester.Model;
using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Beosztasmester.Services;

public interface IRequestsService
{
    Task<IEnumerable<EmployeeRequestDto>> GetRequestsAsync(Guid? employeeId, string? status);
    Task<EmployeeRequestDto?> GetRequestByIdAsync(Guid id);
    Task<EmployeeRequestDto> CreateRequestAsync(CreateEmployeeRequestDto dto);
    Task<bool> UpdateRequestStatusAsync(Guid id, UpdateRequestStatusDto dto);
    Task<bool> DeleteRequestAsync(Guid id);
}

public class RequestsService: IRequestsService
{
    private readonly DataContext _dataContext;

    public RequestsService(DataContext c)
    {
        _dataContext = c;
    }

    public async Task<IEnumerable<EmployeeRequestDto>> GetRequestsAsync(Guid? employeeId, string? status)
    {
        var query = _dataContext.Requests.AsQueryable();

        if (employeeId.HasValue)
        {
            query = query.Where(r => r.Employee_Id == employeeId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(r => r.Status.ToLower() == status.ToLower());
        }

        return await query.Select(r => new EmployeeRequestDto
        {
            Id = r.Id,
            Employee_Id = r.Employee_Id,
            Date = r.Date,
            Type = r.Type,
            Preferred_Shift_Type_Id = r.Shift_Type_Id,
            Priority = r.Priority,
            Status = r.Status
        }).ToListAsync();
    }

    public async Task<EmployeeRequestDto?> GetRequestByIdAsync(Guid id)
    {
        var request = await _dataContext.Requests.FindAsync(id);
        if (request == null) return null;

        return new EmployeeRequestDto
        {
            Id = request.Id,
            Employee_Id = request.Employee_Id,
            Date = request.Date,
            Type = request.Type,
            Preferred_Shift_Type_Id = request.Shift_Type_Id,
            Priority = request.Priority,
            Status = request.Status
        };
    }

    public async Task<EmployeeRequestDto> CreateRequestAsync(CreateEmployeeRequestDto dto)
    {
        var request = new Request
        {
            Id = Guid.NewGuid(),
            Employee_Id = dto.Employee_Id,
            Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc),
            Type = dto.Type, // e.g. "DAY_OFF", "SHIFT_PREFERENCE"
            Shift_Type_Id = dto.Preferred_Shift_Type_Id,
            Priority = dto.Priority,
            Status = "Pending"
        };

        _dataContext.Requests.Add(request);
        await _dataContext.SaveChangesAsync();

        return new EmployeeRequestDto
        {
            Id = request.Id,
            Employee_Id = request.Employee_Id,
            Date = request.Date,
            Type = request.Type,
            Preferred_Shift_Type_Id = request.Shift_Type_Id,
            Priority = request.Priority,
            Status = request.Status
        };
    }

    public async Task<bool> UpdateRequestStatusAsync(Guid id, UpdateRequestStatusDto dto)
    {
        var request = await _dataContext.Requests.FindAsync(id);
        if (request == null) return false;

        request.Status = dto.Status; // e.g. "Approved", "Rejected"
        await _dataContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteRequestAsync(Guid id)
    {
        var request = await _dataContext.Requests.FindAsync(id);
        if (request == null) return false;

        _dataContext.Requests.Remove(request);
        await _dataContext.SaveChangesAsync();

        return true;
    }
}

