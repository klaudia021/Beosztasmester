using Microsoft.EntityFrameworkCore;
using Beosztasmester.Models.Data;

namespace Beosztasmester.Model
{
    public class DataContext:DbContext
    {
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Audit_Entry> Audit_Entries { get; set; }
        public DbSet<Competency> Competencies { get; set; }
        public DbSet<Demand_Template> Demand_Templates { get; set; }
        public DbSet<Demand> Demands { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Employee_Competency> Employee_Competencies { get; set; }
        public DbSet<Horizon> Horizons { get; set; }
        public DbSet<Organizational_Unit> Organizational_Units { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Roster_Version> Roster_Versions { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Shift_Type> Shift_Types { get; set; }
        public DbSet<Unavailability> Unavailabilities { get; set; }


        public DataContext(DbContextOptions options) : base(options) { }
    }
}


/*
 * Claude fordítások
 * -----------------
 * Szervezeti_egység    -   Organizational_Unit
 * Dolgozó	            -   Employee
 * Kompetencia	        -   Competency
 * Dolgozó_kompetencia	-   Employee_Competency
 * Műszaktípus	        -   Shift_Type
 * Elérhetetlenség	    -   Unavailability
 * Kérés	            -   Request
 * Igény_sablon	        -   Demand_Template
 * Igény	            -   Demand
 * Szabály	            -   Rule
 * Horizont	            -   Horizon
 * Beosztás_verzió	    -   Roster_Version
 * Hozzárendelés	    -   Assignment
 * Solver_futás	        -   Solver_Run
 * Auditbejegyzés	    -   Audit_Entry
 */
