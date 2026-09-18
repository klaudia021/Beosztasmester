using System.ComponentModel.DataAnnotations;

namespace Beosztasmester.Models.Data
{
    public class Competency

    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Employee_Competency> Employee_Competencies { get; set; } = new List<Employee_Competency>();
    }
}
