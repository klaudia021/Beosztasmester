using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Assignment
    {
        [Key]
        public int Id { get; set; }
        public int RosterVersionId { get; set; }
        [ForeignKey(nameof(RosterVersionId))]
        public RosterVersion RosterVersion { get; set; } = null!;
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;
        public DateTime Date { get; set; }
        public int ShiftTypeId { get; set; }
        [ForeignKey(nameof(ShiftTypeId))]
        public ShiftType ShiftType { get; set; } = null!;
        public bool IsLocked { get; set; }
        public string Source { get; set; } = "Solver"; //Solver, Manual

    }
}
