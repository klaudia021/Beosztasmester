using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Rule
    {
        [Key] 
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;
        public string TypeCode { get; set; } = string.Empty;
        public string Scope { get; set; } = "Global";
        public int? ScopeRefId { get; set; }
        public bool IsHard { get; set; }
        public int Weight { get; set; }
        [Column(TypeName = "jsonb")]
        public string ParametersJson { get; set; } = "{}";
        public bool IsActive { get; set; }

    }
}
