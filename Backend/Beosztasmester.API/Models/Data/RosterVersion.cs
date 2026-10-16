using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class RosterVersion
    {
        [Key]
        public int Id { get; set; }
        public int HorizonId { get; set; }
        [ForeignKey(nameof(HorizonId))]
        public Horizon Horizon { get; set; } = null!;
        public int VersionNumber { get; set; }
        public string Status { get; set; } = "Draft";
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public double? ObjectiveValue { get; set; }
        [Column(TypeName = "jsonb")]
        public string? SolutionMetadataJson { get; set; }
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();   //for the one to many 
    }
}
