using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Horizon
    {
        [Key]
        public int Id { get; set; }
        public int Organizational_Unit_Id { get; set; }
        [ForeignKey(nameof(Organizational_Unit_Id))]
        public Organizational_Unit Organizational_Unit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public ICollection<Roster_Version> Roster_Versions { get; set; } = new List<Roster_Version>(); //for the one to many 
    }
}
