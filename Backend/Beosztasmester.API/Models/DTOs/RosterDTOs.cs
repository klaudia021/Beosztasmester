namespace Beosztasmester.Models.DTOs;

public class RosterVersionDto
{
    public int Id { get; set; }
    public int HorizonId { get; set; }
    public int VersionNumber { get; set; }
    public string Status { get; set; } = "Draft"; // Draft, Pending, Published, Archived
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public int? ObjectiveValue { get; set; }
    public List<AssignmentDto> Assignments { get; set; } = new();
}

public class RosterDiffDto
{
    public int VersionAId { get; set; }
    public int VersionBId { get; set; }
    public List<AssignmentChangeDto> Changes { get; set; } = new();
}

public class RosterItemDto
{
    public int EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public string ShiftType { get; set; } = string.Empty;
}