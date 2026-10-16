using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public Guid? Parent_Id { get; set; }

        [ForeignKey(nameof(Parent_Id))]
        public Department? Parent_Unit { get; set; }

        public ICollection<Department> Child_Units { get; set; } = new List<Department>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
