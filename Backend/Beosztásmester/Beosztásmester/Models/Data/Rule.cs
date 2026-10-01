using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Rule
    {
        [Key] 
        public Guid Id { get; set; }
        public Guid Department_Id { get; set; }
        [ForeignKey(nameof(Department_Id))]
        public Department Department { get; set; }
        public string Type_Code { get; set; }
        public string Scope { get; set; }
        public string? Scope_Ref { get; set; }
        public bool Is_Hard { get; set; }
        public int Weight { get; set; }
        [Column(TypeName = "jsonb")]
        public string Parameters_Json { get; set; } = "{}";
        public bool Is_Active { get; set; }

    }
}
