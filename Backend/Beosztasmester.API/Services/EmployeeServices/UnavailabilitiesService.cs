using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Beosztasmester.Services;

public interface IUnavailabilitiesService
{
    Task<IEnumerable<UnavailabilityDto>> GetUnavailabilitiesAsync(int? employeeId, DateTime? fromDate, DateTime? toDate);
    Task<UnavailabilityDto?> GetUnavailabilityByIdAsync(int id);
    Task<UnavailabilityDto> CreateUnavailabilityAsync(CreateUnavailabilityDto dto);
    Task<bool> DeleteUnavailabilityAsync(int id);
}
public class UnavailabilitiesService : IUnavailabilitiesService
{
    private readonly DataContext _dataContext;

    public UnavailabilitiesService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<UnavailabilityDto>> GetUnavailabilitiesAsync(int? employeeId, DateTime? fromDate, DateTime? toDate)
    {
        var query = _dataContext.Unavailabilities.AsQueryable();

        if (employeeId.HasValue)
        {
            query = query.Where(u => u.EmployeeId == employeeId.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(u => u.EndDate >= DateTime.SpecifyKind(fromDate.Value, DateTimeKind.Utc));
        }

        if (toDate.HasValue)
        {
            query = query.Where(u => u.StartDate <= DateTime.SpecifyKind(toDate.Value, DateTimeKind.Utc));
        }

        return await query.Select(u => new UnavailabilityDto
        {
            Id = u.Id,
            EmployeeId = u.EmployeeId,
            Type = u.Type,
            StartDate = u.StartDate,
            EndDate = u.EndDate,
            Reason = u.Reason ?? string.Empty
        }).ToListAsync();
    }

    public async Task<UnavailabilityDto?> GetUnavailabilityByIdAsync(int id)
    {
        var u = await _dataContext.Unavailabilities.FindAsync(id);
        if (u == null) return null;

        return new UnavailabilityDto
        {
            Id = u.Id,
            EmployeeId = u.EmployeeId,
            Type = u.Type,
            StartDate = u.StartDate,
            EndDate = u.EndDate,
            Reason = u.Reason ?? string.Empty
        };
    }

    public async Task<UnavailabilityDto> CreateUnavailabilityAsync(CreateUnavailabilityDto dto)
    {
        var unavailability = new Unavailability
        {
            EmployeeId = dto.EmployeeId,
            Type = dto.Type, // e.g. "Vacation", "Sick", "Training"
            StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
            EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
            Reason = dto.Reason
        };

        _dataContext.Unavailabilities.Add(unavailability);
        await _dataContext.SaveChangesAsync();

        return new UnavailabilityDto
        {
            Id = unavailability.Id,
            EmployeeId = unavailability.EmployeeId,
            Type = unavailability.Type,
            StartDate = unavailability.StartDate,
            EndDate = unavailability.EndDate,
            Reason = unavailability.Reason ?? string.Empty
        };
    }

    public async Task<bool> DeleteUnavailabilityAsync(int id)
    {
        var u = await _dataContext.Unavailabilities.FindAsync(id);
        if (u == null) return false;

        _dataContext.Unavailabilities.Remove(u);
        await _dataContext.SaveChangesAsync();

        return true;
    }
}
