namespace Beosztasmester.Models.DTOs;

public class SkillDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class CreateSkillDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}