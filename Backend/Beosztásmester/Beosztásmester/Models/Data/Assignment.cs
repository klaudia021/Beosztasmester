using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Assignment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Version_Id { get; set; } //Roster version???????
        public Guid Employee_Id { get; set; }
        [ForeignKey(nameof(Employee_Id))]
        public Employee Employee { get; set; }
        public DateTime Date { get; set; }
        public Guid Shift_Type_Id { get; set; }
        [ForeignKey(nameof(Shift_Type_Id))]
        public Shift_Type Shift_Type { get; set; }
        public bool IsLocked { get; set; }
        public string Source { get; set; } = "Solver"; //Solver, Manual

    }
}
