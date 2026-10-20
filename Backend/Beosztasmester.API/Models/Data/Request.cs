using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        public DateTime Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // e.g., DAY_OFF, SHIFT_PREFERENCE

        public int? ShiftTypeId { get; set; }

        [ForeignKey(nameof(ShiftTypeId))]
        public ShiftType? ShiftType { get; set; }

        public int Priority { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";
    }
}
