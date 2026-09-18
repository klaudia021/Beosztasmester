using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        public int Employee_Id { get; set; }

        [ForeignKey(nameof(Employee_Id))]
        public Employee Employee { get; set; } = null!;

        public DateTime Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // e.g., DAY_OFF, SHIFT_PREFERENCE

        public int? Shift_Type_Id { get; set; }

        [ForeignKey(nameof(Shift_Type_Id))]
        public Shift_Type? Shift_Type { get; set; }

        public int Priority { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";
    }
}
