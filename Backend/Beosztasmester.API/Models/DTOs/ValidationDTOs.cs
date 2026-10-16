namespace Beosztasmester.Models.DTOs;

public class ValidationResultDto
{
    public bool IsValid { get; set; }
    public List<RuleViolationDto> HardViolations { get; set; } = new();
    public List<RuleViolationDto> SoftViolations { get; set; } = new();
    public int TotalSoftPenalty { get; set; }
}

public class RuleViolationDto
{
    public string RuleTypeCode { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Guid? AffectedEmployeeId { get; set; }
    public DateTime? AffectedDate { get; set; }
}