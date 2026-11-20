using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Demand
    {
        [Key]
        public Guid Id { get; set; }

        public Guid DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;

        public DateTime Date { get; set; }

        public Guid ShiftTypeId { get; set; }

        [ForeignKey(nameof(ShiftTypeId))]
        public ShiftType ShiftType { get; set; } = null!;

        public Guid? CompetencyId { get; set; }

        [ForeignKey(nameof(CompetencyId))]
        public Competency? Competency { get; set; }

        public int MinHeadcount { get; set; }

        public int? MaxHeadcount { get; set; }
    }
}
