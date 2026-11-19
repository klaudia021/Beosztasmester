using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Assignment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RosterVersionId { get; set; }
        [ForeignKey(nameof(RosterVersionId))]
        public RosterVersion RosterVersion { get; set; } = null!;
        public Guid EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;
        public DateTime Date { get; set; }
        public Guid ShiftTypeId { get; set; }
        [ForeignKey(nameof(ShiftTypeId))]
        public ShiftType ShiftType { get; set; } = null!;
        public bool IsLocked { get; set; }
        public string Source { get; set; } = "Solver"; //Solver, Manual

    }
}
