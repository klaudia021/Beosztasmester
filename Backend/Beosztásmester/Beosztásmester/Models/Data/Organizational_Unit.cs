using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Organizational_Unit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public int? Parent_Id { get; set; }

        [ForeignKey(nameof(Parent_Id))]
        public Organizational_Unit? Parent_Unit { get; set; }

        public ICollection<Organizational_Unit> Child_Units { get; set; } = new List<Organizational_Unit>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
