using System.ComponentModel.DataAnnotations;

namespace Beosztasmester.Models.DTOs
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public Guid Department_Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "Active"; // Active, Inactive, etc.
        public int Contract_Hours_Per_Week { get; set; }
        public DateTime Entry_Date { get; set; }
        public DateTime? Exit_Date { get; set; }
        public List<Guid> SkillIds { get; set; } = new();
    }

    public class CreateEmployeeDto
    {
        [Required]
        public Guid Department_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 168)]
        public int Contract_Hours_Per_Week { get; set; } = 40;

        public DateTime Entry_Date { get; set; } = DateTime.UtcNow;

        public List<Guid> SkillIds { get; set; } = new();
    }

    public class UpdateEmployeeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        public int Contract_Hours_Per_Week { get; set; }
        public DateTime? Exit_Date { get; set; }
        public List<Guid> SkillIds { get; set; } = new();
    }
}
