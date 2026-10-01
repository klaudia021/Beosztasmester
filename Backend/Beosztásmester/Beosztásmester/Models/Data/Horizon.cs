using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Horizon
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Department_Id { get; set; }
        [ForeignKey(nameof(Department_Id))]
        public Department Department { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public ICollection<Roster_Version> Roster_Versions { get; set; } = new List<Roster_Version>(); //for the one to many 
    }
}
