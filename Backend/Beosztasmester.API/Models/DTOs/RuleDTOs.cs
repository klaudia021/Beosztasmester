namespace Beosztasmester.Models.DTOs;

public class RuleDto
{
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    public string TypeCode { get; set; } = string.Empty; // e.g. MAX_CONSECUTIVE_NIGHTS
    public string Scope { get; set; } = "Global"; // Global, Department, Employee
    public int? ScopeRefId { get; set; }
    public bool IsHard { get; set; }
    public int Weight { get; set; }
    public string ParametersJson { get; set; } = "{}";
    public bool IsActive { get; set; }
}

public class CreateRuleDto
{
    public int DepartmentId { get; set; }
    public string TypeCode { get; set; } = string.Empty;
    public string Scope { get; set; } = "Global";
    public int? ScopeRefId { get; set; }
    public bool IsHard { get; set; }
    public int Weight { get; set; }
    public string ParametersJson { get; set; } = "{}";
}