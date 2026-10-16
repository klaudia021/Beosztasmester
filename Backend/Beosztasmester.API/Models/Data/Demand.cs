using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Demand
    {
        [Key]
        public int Id { get; set; }

        public int Department_Id { get; set; }

        [ForeignKey(nameof(Department_Id))]
        public Department Department { get; set; } = null!;

        public DateTime Date { get; set; }

        public int Shift_Type_Id { get; set; }

        [ForeignKey(nameof(Shift_Type_Id))]
        public Shift_Type Shift_Type { get; set; } = null!;

        public int? Competency_Id { get; set; }

        [ForeignKey(nameof(Competency_Id))]
        public Competency? Competency { get; set; }

        public int Min_Headcount { get; set; }

        public int? Max_Headcount { get; set; }
    }
}
