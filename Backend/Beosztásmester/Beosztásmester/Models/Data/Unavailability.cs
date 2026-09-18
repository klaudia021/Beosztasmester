using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Unavailability
    {
        [Key]
        public int Id { get; set; }

        public int Employee_Id { get; set; }

        [ForeignKey(nameof(Employee_Id))]
        public Employee Employee { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // e.g., Vacation, Illness

        public DateTime Start_Date { get; set; }

        public DateTime End_Date { get; set; }

        public string? Reason { get; set; }
    }
}
