using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Rule
    {
        [Key] 
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;
        public string TypeCode { get; set; } = string.Empty;
        public string Scope { get; set; } = "Global";
        public Guid? ScopeRefId { get; set; }
        public bool IsHard { get; set; }
        public int Weight { get; set; }
        [Column(TypeName = "jsonb")]
        public string ParametersJson { get; set; } = "{}";
        public bool IsActive { get; set; }

    }
}
