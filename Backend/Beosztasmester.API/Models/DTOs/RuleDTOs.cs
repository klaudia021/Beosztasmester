namespace Beosztasmester.Models.DTOs;

public class RuleDto
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public string TypeCode { get; set; } = string.Empty; // e.g. MAX_CONSECUTIVE_NIGHTS
    public string Scope { get; set; } = "Global"; // Global, Department, Employee
    public Guid? ScopeRefId { get; set; }
    public bool IsHard { get; set; }
    public int Weight { get; set; }
    public string ParametersJson { get; set; } = "{}";
    public bool IsActive { get; set; }
}

public class CreateRuleDto
{
    public Guid DepartmentId { get; set; }
    public string TypeCode { get; set; } = string.Empty;
    public string Scope { get; set; } = "Global";
    public Guid? ScopeRefId { get; set; }
    public bool IsHard { get; set; }
    public int Weight { get; set; }
    public string ParametersJson { get; set; } = "{}";
}