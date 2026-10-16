using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class AuditEntry
    {
        [Key]
        public int Id { get; set; }
        public string Entity { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string Operation { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "jsonb")]
        public string ChangesJson { get; set; } = "{}";
    }
}
