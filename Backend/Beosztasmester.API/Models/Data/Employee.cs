using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Active";

        public int ContractHoursPerWeek { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime? ExitDate { get; set; }

        // Navigation properties
        public ICollection<EmployeeCompetency> EmployeeCompetencies { get; set; } = new List<EmployeeCompetency>();
        public ICollection<Unavailability> Unavailabilities { get; set; } = new List<Unavailability>();
        public ICollection<Request> Requests { get; set; } = new List<Request>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
