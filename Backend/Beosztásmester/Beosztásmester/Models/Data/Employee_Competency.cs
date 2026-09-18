using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Employee_Competency
    {
        [Key]
        public int Id { get; set; }
        public int Employee_Id { get; set; }

        [ForeignKey(nameof(Employee_Id))]
        public Employee Employee { get; set; } = null!;

        public int Competency_Id { get; set; }

        [ForeignKey(nameof(Competency_Id))]
        public Competency Competency { get; set; } = null!;

        public DateTime Valid_From { get; set; }

        public DateTime? Valid_To { get; set; }
    }
}
