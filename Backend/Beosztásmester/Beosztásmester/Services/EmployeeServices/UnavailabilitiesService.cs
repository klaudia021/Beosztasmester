using Beosztasmester.Model;
using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Beosztasmester.Services;

public interface IUnavailabilitiesService
{
    Task<IEnumerable<UnavailabilityDto>> GetUnavailabilitiesAsync(Guid? employeeId, DateTime? fromDate, DateTime? toDate);
    Task<UnavailabilityDto?> GetUnavailabilityByIdAsync(Guid id);
    Task<UnavailabilityDto> CreateUnavailabilityAsync(CreateUnavailabilityDto dto);
    Task<bool> DeleteUnavailabilityAsync(Guid id);
}
public class UnavailabilitiesService : IUnavailabilitiesService
{
    private readonly DataContext _dataContext;

    public UnavailabilitiesService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<UnavailabilityDto>> GetUnavailabilitiesAsync(Guid? employeeId, DateTime? fromDate, DateTime? toDate)
    {
        var query = _dataContext.Unavailabilities.AsQueryable();

        if (employeeId.HasValue)
        {
            query = query.Where(u => u.Employee_Id == employeeId.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(u => u.End_Date >= DateTime.SpecifyKind(fromDate.Value, DateTimeKind.Utc));
        }

        if (toDate.HasValue)
        {
            query = query.Where(u => u.Start_Date <= DateTime.SpecifyKind(toDate.Value, DateTimeKind.Utc));
        }

        return await query.Select(u => new UnavailabilityDto
        {
            Id = u.Id,
            EmployeeId = u.Employee_Id,
            Type = u.Type,
            StartDate = u.Start_Date,
            EndDate = u.End_Date,
            Reason = u.Reason
        }).ToListAsync();
    }

    public async Task<UnavailabilityDto?> GetUnavailabilityByIdAsync(Guid id)
    {
        var u = await _dataContext.Unavailabilities.FindAsync(id);
        if (u == null) return null;

        return new UnavailabilityDto
        {
            Id = u.Id,
            EmployeeId = u.Employee_Id,
            Type = u.Type,
            StartDate = u.Start_Date,
            EndDate = u.End_Date,
            Reason = u.Reason
        };
    }

    public async Task<UnavailabilityDto> CreateUnavailabilityAsync(CreateUnavailabilityDto dto)
    {
        var unavailability = new Unavailability
        {
            Id = Guid.NewGuid(),
            Employee_Id = dto.EmployeeId,
            Type = dto.Type, // e.g. "Vacation", "Sick", "Training"
            Start_Date = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
            End_Date = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
            Reason = dto.Reason
        };

        _dataContext.Unavailabilities.Add(unavailability);
        await _dataContext.SaveChangesAsync();

        return new UnavailabilityDto
        {
            Id = unavailability.Id,
            EmployeeId = unavailability.Employee_Id,
            Type = unavailability.Type,
            StartDate = unavailability.Start_Date,
            EndDate = unavailability.End_Date,
            Reason = unavailability.Reason
        };
    }

    public async Task<bool> DeleteUnavailabilityAsync(Guid id)
    {
        var u = await _dataContext.Unavailabilities.FindAsync(id);
        if (u == null) return false;

        _dataContext.Unavailabilities.Remove(u);
        await _dataContext.SaveChangesAsync();

        return true;
    }
}

