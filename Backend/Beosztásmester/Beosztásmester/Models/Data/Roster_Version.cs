using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Roster_Version
    {
        [Key]
        public int Id { get; set; }
        public int Horizon_Id { get; set; }
        [ForeignKey(nameof(Horizon_Id))]
        public Horizon Horizon { get; set; }
        public int Version_Number { get; set; }
        public string Status { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public double? Objective_Value { get; set; }
        [Column(TypeName = "jsonb")]
        public string? Solution_Metadata_Json { get; set; }
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();   //for the one to many 
    }
}
