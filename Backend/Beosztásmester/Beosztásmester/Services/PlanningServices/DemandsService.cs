using Beosztasmester.Model;
using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Beosztasmester.Services;

public interface IDemandsService
{
    Task<IEnumerable<DemandDto>> GetDemandsAsync(Guid departmentId, DateTime? fromDate, DateTime? toDate);
    Task<DemandDto?> GetDemandByIdAsync(Guid id);
    Task<DemandDto> CreateDemandAsync(CreateDemandDto dto);
    Task<IEnumerable<DemandDto>> BulkUpsertDemandsAsync(Guid departmentId, List<CreateDemandDto> dtos);
    Task<bool> DeleteDemandAsync(Guid id);
}

public class DemandsService : IDemandsService
{
    private readonly DataContext _dataContext;

    public DemandsService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<DemandDto>> GetDemandsAsync(Guid departmentId, DateTime? fromDate, DateTime? toDate)
    {
        var query = _dataContext.Demands.Where(d => d.Department_Id == departmentId);

        if (fromDate.HasValue)
        {
            query = query.Where(d => d.Date >= DateTime.SpecifyKind(fromDate.Value, DateTimeKind.Utc));
        }

        if (toDate.HasValue)
        {
            query = query.Where(d => d.Date <= DateTime.SpecifyKind(toDate.Value, DateTimeKind.Utc));
        }

        return await query.Select(d => new DemandDto
        {
            Id = d.Id,
            DepartmentId = d.Department_Id,
            Date = d.Date,
            ShiftTypeId = d.Shift_Type_Id,
            RequiredSkillId = d.Competency_Id,
            MinEmployees = d.Min_Headcount,
            MaxEmployees = d.Max_Headcount
        }).ToListAsync();
    }

    public async Task<DemandDto?> GetDemandByIdAsync(Guid id)
    {
        var d = await _dataContext.Demands.FindAsync(id);
        if (d == null) return null;

        return new DemandDto
        {
            Id = d.Id,
            DepartmentId = d.Department_Id,
            Date = d.Date,
            ShiftTypeId = d.Shift_Type_Id,
            RequiredSkillId = d.Competency_Id,
            MinEmployees = d.Min_Headcount,
            MaxEmployees = d.Max_Headcount
        };
    }

    public async Task<DemandDto> CreateDemandAsync(CreateDemandDto dto)
    {
        var demand = new Demand
        {
            Id = Guid.NewGuid(),
            Department_Id = dto.DepartmentId,
            Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc),
            Shift_Type_Id = dto.ShiftTypeId,
            Competency_Id = dto.RequiredSkillId,
            Min_Headcount = dto.MinEmployees,
            Max_Headcount = dto.MaxEmployees
        };

        _dataContext.Demands.Add(demand);
        await _dataContext.SaveChangesAsync();

        return new DemandDto
        {
            Id = demand.Id,
            DepartmentId = demand.Department_Id,
            Date = demand.Date,
            ShiftTypeId = demand.Shift_Type_Id,
            RequiredSkillId = demand.Competency_Id,
            MinEmployees = demand.Min_Headcount,
            MaxEmployees = demand.Max_Headcount
        };
    }

    /// <summary>
    /// Tömeges igényfrissítés/hozzáadás (pénztáros/műszak mátrixos tervezőfelülethez)
    /// </summary>
    public async Task<IEnumerable<DemandDto>> BulkUpsertDemandsAsync(Guid departmentId, List<CreateDemandDto> dtos)
    {
        var resultList = new List<DemandDto>();

        foreach (var dto in dtos)
        {
            var dateUtc = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc);

            var existing = await _dataContext.Demands
                .FirstOrDefaultAsync(d => d.Department_Id == departmentId
                                       && d.Date.Date == dateUtc.Date
                                       && d.Shift_Type_Id == dto.ShiftTypeId
                                       && d.Competency_Id == dto.RequiredSkillId);

            if (existing != null)
            {
                existing.Min_Headcount = dto.MinEmployees;
                existing.Max_Headcount = dto.MaxEmployees;

                resultList.Add(new DemandDto
                {
                    Id = existing.Id,
                    DepartmentId = existing.Department_Id,
                    Date = existing.Date,
                    ShiftTypeId = existing.Shift_Type_Id,
                    RequiredSkillId = existing.Competency_Id,
                    MinEmployees = existing.Min_Headcount,
                    MaxEmployees = existing.Max_Headcount
                });
            }
            else
            {
                var newDemand = new Demand
                {
                    Id = Guid.NewGuid(),
                    Department_Id = departmentId,
                    Date = dateUtc,
                    Shift_Type_Id = dto.ShiftTypeId,
                    Competency_Id = dto.RequiredSkillId,
                    Min_Headcount = dto.MinEmployees,
                    Max_Headcount = dto.MaxEmployees
                };

                _dataContext.Demands.Add(newDemand);
                resultList.Add(new DemandDto
                {
                    Id = newDemand.Id,
                    DepartmentId = newDemand.Department_Id,
                    Date = newDemand.Date,
                    ShiftTypeId = newDemand.Shift_Type_Id,
                    RequiredSkillId = newDemand.Competency_Id,
                    MinEmployees = newDemand.Min_Headcount,
                    MaxEmployees = newDemand.Max_Headcount
                });
            }
        }

        await _dataContext.SaveChangesAsync();
        return resultList;
    }

    public async Task<bool> DeleteDemandAsync(Guid id)
    {
        var demand = await _dataContext.Demands.FindAsync(id);
        if (demand == null) return false;

        _dataContext.Demands.Remove(demand);
        await _dataContext.SaveChangesAsync();
        return true;
    }
}