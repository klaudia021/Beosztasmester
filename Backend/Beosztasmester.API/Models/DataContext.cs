using Microsoft.EntityFrameworkCore;
using Beosztasmester.Models.Data;

namespace Beosztasmester.Models;

public class DataContext : DbContext
{
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AuditEntry> AuditEntries { get; set; }
    public DbSet<Competency> Competencies { get; set; }
    public DbSet<DemandTemplate> DemandTemplates { get; set; }
    public DbSet<Demand> Demands { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeCompetency> EmployeeCompetencies { get; set; }
    public DbSet<Horizon> Horizons { get; set; }
    public DbSet<Department> Department { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<RosterVersion> RosterVersions { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<ShiftType> ShiftTypes { get; set; }
    public DbSet<Unavailability> Unavailabilities { get; set; }
    public DbSet<SolverRun> SolverRuns { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}


/*
 * Claude fordítások
 * -----------------
 * Szervezeti_egység    -   Organizational_Unit
 * Dolgozó	            -   Employee
 * Kompetencia	        -   Competency
 * Dolgozó_kompetencia	-   EmployeeCompetency
 * Műszaktípus	        -   ShiftType
 * Elérhetetlenség	    -   Unavailability
 * Kérés	            -   Request
 * Igény_sablon	        -   DemandTemplate
 * Igény	            -   Demand
 * Szabály	            -   Rule
 * Horizont	            -   Horizon
 * Beosztás_verzió	    -   RosterVersion
 * Hozzárendelés	    -   Assignment
 * Solver_futás	        -   SolverRun
 * Auditbejegyzés	    -   AuditEntry
 */
