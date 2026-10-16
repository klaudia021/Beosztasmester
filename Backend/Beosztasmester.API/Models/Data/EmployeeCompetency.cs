using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class EmployeeCompetency
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        public int CompetencyId { get; set; }

        [ForeignKey(nameof(CompetencyId))]
        public Competency Competency { get; set; } = null!;

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }
    }
}
