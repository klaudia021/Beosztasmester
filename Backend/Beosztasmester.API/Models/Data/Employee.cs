using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        public Guid Department_Id { get; set; }

        [ForeignKey(nameof(Department_Id))]
        public Department Department { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Active";

        public int Contract_Hours_Per_Week { get; set; }

        public DateTime Entry_Date { get; set; }

        public DateTime? Exit_Date { get; set; }

        // Navigation properties
        public ICollection<Employee_Competency> Employee_Competencies { get; set; } = new List<Employee_Competency>();
        public ICollection<Unavailability> Unavailabilities { get; set; } = new List<Unavailability>();
        public ICollection<Request> Requests { get; set; } = new List<Request>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
