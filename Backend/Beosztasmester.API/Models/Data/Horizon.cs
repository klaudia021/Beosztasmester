using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Horizon
    {
        [Key]
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "DRAFT";
        public ICollection<RosterVersion> RosterVersions { get; set; } = new List<RosterVersion>(); //for the one to many
    }
}
