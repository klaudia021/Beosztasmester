using Beosztasmester.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace Beosztasmester.Models;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(DataContext context, CancellationToken cancellationToken = default)
    {
        if (await context.Department.AnyAsync(cancellationToken))
        {
            return;
        }

        var departments = Enumerable.Range(1, 5)
            .Select(index => new Department { Name = $"Department {index}" })
            .ToArray();
        context.Department.AddRange(departments);
        await context.SaveChangesAsync(cancellationToken);

        departments[1].ParentId = departments[0].Id;
        departments[2].ParentId = departments[0].Id;
        await context.SaveChangesAsync(cancellationToken);

        var competencies = new[]
        {
            new Competency { Code = "CASHIER", Name = "Cashier" },
            new Competency { Code = "STOCK", Name = "Stock handling" },
            new Competency { Code = "SUPERVISOR", Name = "Supervisor" },
            new Competency { Code = "FIRSTAID", Name = "First aid" },
            new Competency { Code = "FORKLIFT", Name = "Forklift operation" }
        };
        context.Competencies.AddRange(competencies);
        await context.SaveChangesAsync(cancellationToken);

        var employees = Enumerable.Range(1, 10)
            .Select(index => new Employee
            {
                DepartmentId = departments[(index - 1) % departments.Length].Id,
                Name = $"Employee {index:00}",
                Status = "Active",
                ContractHoursPerWeek = index % 2 == 0 ? 40 : 32,
                EntryDate = DateTime.UtcNow.Date.AddDays(-index * 30)
            })
            .ToArray();
        context.Employees.AddRange(employees);
        await context.SaveChangesAsync(cancellationToken);

        var shiftTypes = new[]
        {
            new ShiftType { DepartmentId = departments[0].Id, Code = "M", StartTime = TimeSpan.FromHours(6), EndTime = TimeSpan.FromHours(14), DurationMinutes = 480 },
            new ShiftType { DepartmentId = departments[0].Id, Code = "A", StartTime = TimeSpan.FromHours(14), EndTime = TimeSpan.FromHours(22), DurationMinutes = 480 },
            new ShiftType { DepartmentId = departments[1].Id, Code = "N", StartTime = TimeSpan.FromHours(22), EndTime = TimeSpan.FromHours(6), DurationMinutes = 480, IsNightShift = true },
            new ShiftType { DepartmentId = departments[2].Id, Code = "D", StartTime = TimeSpan.FromHours(8), EndTime = TimeSpan.FromHours(16), DurationMinutes = 480 },
            new ShiftType { DepartmentId = departments[3].Id, Code = "P", StartTime = TimeSpan.FromHours(10), EndTime = TimeSpan.FromHours(18), DurationMinutes = 480 }
        };
        context.ShiftTypes.AddRange(shiftTypes);
        await context.SaveChangesAsync(cancellationToken);

        var employeeCompetencies = Enumerable.Range(0, 10)
            .Select(index => new EmployeeCompetency
            {
                EmployeeId = employees[index].Id,
                CompetencyId = competencies[index % competencies.Length].Id,
                ValidFrom = DateTime.UtcNow.Date.AddYears(-1)
            })
            .ToArray();
        context.EmployeeCompetencies.AddRange(employeeCompetencies);

        var baseDate = DateTime.UtcNow.Date;
        var horizons = Enumerable.Range(0, 5)
            .Select(index => new Horizon
            {
                DepartmentId = departments[index].Id,
                StartDate = baseDate.AddDays(index * 7),
                EndDate = baseDate.AddDays(index * 7 + 27),
                Status = index == 0 ? "DRAFT" : "ACTIVE"
            })
            .ToArray();
        context.Horizons.AddRange(horizons);
        await context.SaveChangesAsync(cancellationToken);

        var rosterVersions = Enumerable.Range(0, 5)
            .Select(index => new RosterVersion
            {
                HorizonId = horizons[index].Id,
                VersionNumber = 1,
                Status = index == 0 ? "Draft" : "Published",
                CreatedBy = "Seeder",
                CreatedDate = DateTime.UtcNow,
                ObjectiveValue = 100 + index
            })
            .ToArray();
        context.RosterVersions.AddRange(rosterVersions);

        var demands = Enumerable.Range(0, 5)
            .Select(index => new Demand
            {
                DepartmentId = departments[index].Id,
                Date = baseDate.AddDays(index),
                ShiftTypeId = shiftTypes[index].Id,
                CompetencyId = competencies[index].Id,
                MinHeadcount = 1,
                MaxHeadcount = 3
            })
            .ToArray();
        context.Demands.AddRange(demands);

        var demandTemplates = Enumerable.Range(0, 5)
            .Select(index => new DemandTemplate
            {
                DepartmentId = departments[index].Id,
                DayOfWeek = (DayOfWeek)(index + 1),
                ShiftTypeId = shiftTypes[index].Id,
                CompetencyId = competencies[index].Id,
                MinHeadcount = 1,
                MaxHeadcount = 2
            })
            .ToArray();
        context.DemandTemplates.AddRange(demandTemplates);

        var requests = Enumerable.Range(0, 5)
            .Select(index => new Request
            {
                EmployeeId = employees[index].Id,
                Date = baseDate.AddDays(index + 1),
                Type = "SHIFT_PREFERENCE",
                ShiftTypeId = shiftTypes[index].Id,
                Priority = index + 1,
                Status = "Pending"
            })
            .ToArray();
        context.Requests.AddRange(requests);

        var unavailabilities = Enumerable.Range(0, 5)
            .Select(index => new Unavailability
            {
                EmployeeId = employees[index + 5].Id,
                Type = "Vacation",
                StartDate = baseDate.AddDays(index + 10),
                EndDate = baseDate.AddDays(index + 12),
                Reason = "Seeded sample data"
            })
            .ToArray();
        context.Unavailabilities.AddRange(unavailabilities);

        var rules = Enumerable.Range(0, 5)
            .Select(index => new Rule
            {
                DepartmentId = departments[index].Id,
                TypeCode = $"MAX_CONSECUTIVE_NIGHTS_{index + 1}",
                Scope = "Department",
                IsHard = index < 3,
                Weight = 10 + index,
                ParametersJson = """{"max":3}""",
                IsActive = true
            })
            .ToArray();
        context.Rules.AddRange(rules);

        var assignments = Enumerable.Range(0, 10)
            .Select(index => new Assignment
            {
                RosterVersion = rosterVersions[index % rosterVersions.Length],
                EmployeeId = employees[index].Id,
                Date = baseDate.AddDays(index / 2),
                ShiftTypeId = shiftTypes[index % shiftTypes.Length].Id,
                IsLocked = index % 2 == 0,
                Source = index % 2 == 0 ? "Manual" : "Solver"
            })
            .ToArray();
        context.Assignments.AddRange(assignments);

        var auditEntries = Enumerable.Range(1, 5)
            .Select(index => new AuditEntry
            {
                Entity = nameof(Employee),
                EntityId = employees[index - 1].Id,
                Operation = "Seed",
                UserId = index,
                ChangesJson = """{"source":"dummy seed data"}"""
            })
            .ToArray();
        context.AuditEntries.AddRange(auditEntries);

        var solverRuns = Enumerable.Range(0, 5)
            .Select(index => new SolverRun
            {
                HorizonId = horizons[index].Id,
                Status = index == 0 ? "Completed" : "Queued"
            })
            .ToArray();
        context.SolverRuns.AddRange(solverRuns);

        await context.SaveChangesAsync(cancellationToken);
    }
}
