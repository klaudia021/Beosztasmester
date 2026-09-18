using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Audit_Entry
    {
        [Key]
        public int Id { get; set; }
        public string Entity {  get; set; }
        public int Entity_Id { get; set; }
        public string Operation {  get; set; }
        public int User_Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "jsonb")]
        public string ChangesJson { get; set; } = "{}";
    }
}
