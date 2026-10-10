namespace Beosztasmester.Models.DTOs;

public class ParseRuleRequestDto
{
    public Guid DepartmentId { get; set; }
    public string NaturalText { get; set; } = string.Empty;
}

public class ParseRuleResponseDto
{
    public bool NeedsClarification { get; set; }
    public string? ClarificationQuestion { get; set; }
    public string HumanReadableDescription { get; set; } = string.Empty;
    public CreateRuleDto? ExtractedRule { get; set; }
}