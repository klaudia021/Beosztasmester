using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Shift_Type
    {
        [Key]
        public int Id { get; set; }

        public int Organizational_Unit_Id { get; set; }

        [ForeignKey(nameof(Organizational_Unit_Id))]
        public Organizational_Unit Organizational_Unit { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string Code { get; set; } = string.Empty;

        public TimeSpan Start_Time { get; set; }

        public TimeSpan End_Time { get; set; }

        public int Duration_Minutes { get; set; }

        public bool Is_Night_Shift { get; set; }
    }
}
