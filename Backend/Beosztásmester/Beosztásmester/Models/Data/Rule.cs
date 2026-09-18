using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beosztasmester.Models.Data
{
    public class Rule
    {
        [Key] 
        public int Id { get; set; }
        public int Organizational_Unit_Id { get; set; }
        [ForeignKey(nameof(Organizational_Unit_Id))]
        public Organizational_Unit Organizational_Unit { get; set; }
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
