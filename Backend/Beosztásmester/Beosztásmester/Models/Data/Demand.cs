using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Demand
    {
        [Key]
        public Guid Id { get; set; }

        public Guid Department_Id { get; set; }

        [ForeignKey(nameof(Department_Id))]
        public Department Department { get; set; } = null!;

        public DateTime Date { get; set; }

        public Guid Shift_Type_Id { get; set; }

        [ForeignKey(nameof(Shift_Type_Id))]
        public Shift_Type ShiftType { get; set; } = null!;

        public Guid? Competency_Id { get; set; }

        [ForeignKey(nameof(Competency_Id))]
        public Competency? Competency { get; set; }

        public int Min_Headcount { get; set; }

        public int? Max_Headcount { get; set; }
    }
}
