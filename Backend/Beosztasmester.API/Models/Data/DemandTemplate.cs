using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class DemandTemplate
    {
        [Key]
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;

        public DayOfWeek DayOfWeek { get; set; }

        public int ShiftTypeId { get; set; }

        [ForeignKey(nameof(ShiftTypeId))]
        public ShiftType ShiftType { get; set; } = null!;

        public int? CompetencyId { get; set; }

        [ForeignKey(nameof(CompetencyId))]
        public Competency? Competency { get; set; }

        public int MinHeadcount { get; set; }

        public int? MaxHeadcount { get; set; }
    }
}
