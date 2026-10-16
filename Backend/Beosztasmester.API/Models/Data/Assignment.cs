using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Assignment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Roster_Version_Id { get; set; }
        [ForeignKey(nameof(Roster_Version_Id))]
        public Roster_Version Roster_Version { get; set; } = null!;
        public Guid Employee_Id { get; set; }
        [ForeignKey(nameof(Employee_Id))]
        public Employee Employee { get; set; } = null!;
        public DateTime Date { get; set; }
        public Guid Shift_Type_Id { get; set; }
        [ForeignKey(nameof(Shift_Type_Id))]
        public Shift_Type Shift_Type { get; set; } = null!;
        public bool IsLocked { get; set; }
        public string Source { get; set; } = "Solver"; //Solver, Manual

    }
}
