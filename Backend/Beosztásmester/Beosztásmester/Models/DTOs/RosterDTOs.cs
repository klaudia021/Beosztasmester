namespace Beosztasmester.Models.DTOs;

public class RosterVersionDto
{
    public Guid Id { get; set; }
    public Guid HorizonId { get; set; }
    public int VersionNumber { get; set; }
    public string Status { get; set; } = "Draft"; // Draft, Pending, Published, Archived
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public int? ObjectiveValue { get; set; }
    public List<AssignmentDto> Assignments { get; set; } = new();
}

public class RosterDiffDto
{
    public Guid VersionAId { get; set; }
    public Guid VersionBId { get; set; }
    public List<AssignmentChangeDto> Changes { get; set; } = new();
}