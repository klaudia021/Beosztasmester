using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class AuditEntry
    {
        [Key]
        public Guid Id { get; set; }
        public string Entity { get; set; } = string.Empty;
        public Guid EntityId { get; set; }
        public string Operation { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "jsonb")]
        public string ChangesJson { get; set; } = "{}";
    }
}
