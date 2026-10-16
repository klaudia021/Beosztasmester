using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Beosztasmester.Services;

public interface IDemandsService
{
    Task<IEnumerable<DemandDto>> GetDemandsAsync(int departmentId, DateTime? fromDate, DateTime? toDate);
    Task<DemandDto?> GetDemandByIdAsync(int id);
    Task<DemandDto> CreateDemandAsync(CreateDemandDto dto);
    Task<IEnumerable<DemandDto>> BulkUpsertDemandsAsync(int departmentId, List<CreateDemandDto> dtos);
    Task<bool> DeleteDemandAsync(int id);
}

public class DemandsService : IDemandsService
{
    private readonly DataContext _dataContext;

    public DemandsService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<DemandDto>> GetDemandsAsync(int departmentId, DateTime? fromDate, DateTime? toDate)
    {
        var query = _dataContext.Demands.Where(d => d.DepartmentId == departmentId);

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
            DepartmentId = d.DepartmentId,
            Date = d.Date,
            ShiftTypeId = d.ShiftTypeId,
            RequiredSkillId = d.CompetencyId,
            MinEmployees = d.MinHeadcount,
            MaxEmployees = d.MaxHeadcount
        }).ToListAsync();
    }

    public async Task<DemandDto?> GetDemandByIdAsync(int id)
    {
        var d = await _dataContext.Demands.FindAsync(id);
        if (d == null) return null;

        return new DemandDto
        {
            Id = d.Id,
            DepartmentId = d.DepartmentId,
            Date = d.Date,
            ShiftTypeId = d.ShiftTypeId,
            RequiredSkillId = d.CompetencyId,
            MinEmployees = d.MinHeadcount,
            MaxEmployees = d.MaxHeadcount
        };
    }

    public async Task<DemandDto> CreateDemandAsync(CreateDemandDto dto)
    {
        var demand = new Demand
        {
            DepartmentId = dto.DepartmentId,
            Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc),
            ShiftTypeId = dto.ShiftTypeId,
            CompetencyId = dto.RequiredSkillId,
            MinHeadcount = dto.MinEmployees,
            MaxHeadcount = dto.MaxEmployees
        };

        _dataContext.Demands.Add(demand);
        await _dataContext.SaveChangesAsync();

        return new DemandDto
        {
            Id = demand.Id,
            DepartmentId = demand.DepartmentId,
            Date = demand.Date,
            ShiftTypeId = demand.ShiftTypeId,
            RequiredSkillId = demand.CompetencyId,
            MinEmployees = demand.MinHeadcount,
            MaxEmployees = demand.MaxHeadcount
        };
    }

    /// <summary>
    /// Tömeges igényfrissítés/hozzáadás (pénztáros/műszak mátrixos tervezőfelülethez)
    /// </summary>
    public async Task<IEnumerable<DemandDto>> BulkUpsertDemandsAsync(int departmentId, List<CreateDemandDto> dtos)
    {
        var resultList = new List<DemandDto>();

        foreach (var dto in dtos)
        {
            var dateUtc = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc);

            var existing = await _dataContext.Demands
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId
                                       && d.Date.Date == dateUtc.Date
                                       && d.ShiftTypeId == dto.ShiftTypeId
                                       && d.CompetencyId == dto.RequiredSkillId);

            if (existing != null)
            {
                existing.MinHeadcount = dto.MinEmployees;
                existing.MaxHeadcount = dto.MaxEmployees;

                resultList.Add(new DemandDto
                {
                    Id = existing.Id,
                    DepartmentId = existing.DepartmentId,
                    Date = existing.Date,
                    ShiftTypeId = existing.ShiftTypeId,
                    RequiredSkillId = existing.CompetencyId,
                    MinEmployees = existing.MinHeadcount,
                    MaxEmployees = existing.MaxHeadcount
                });
            }
            else
            {
                var newDemand = new Demand
                {
                    DepartmentId = departmentId,
                    Date = dateUtc,
                    ShiftTypeId = dto.ShiftTypeId,
                    CompetencyId = dto.RequiredSkillId,
                    MinHeadcount = dto.MinEmployees,
                    MaxHeadcount = dto.MaxEmployees
                };

                _dataContext.Demands.Add(newDemand);
                resultList.Add(new DemandDto
                {
                    Id = newDemand.Id,
                    DepartmentId = newDemand.DepartmentId,
                    Date = newDemand.Date,
                    ShiftTypeId = newDemand.ShiftTypeId,
                    RequiredSkillId = newDemand.CompetencyId,
                    MinEmployees = newDemand.MinHeadcount,
                    MaxEmployees = newDemand.MaxHeadcount
                });
            }
        }

        await _dataContext.SaveChangesAsync();
        return resultList;
    }

    public async Task<bool> DeleteDemandAsync(int id)
    {
        var demand = await _dataContext.Demands.FindAsync(id);
        if (demand == null) return false;

        _dataContext.Demands.Remove(demand);
        await _dataContext.SaveChangesAsync();
        return true;
    }
}