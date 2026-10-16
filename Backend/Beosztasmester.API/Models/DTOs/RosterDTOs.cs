namespace Beosztasmester.Models.DTOs;

public class RosterVersionDto
{
    public int Id { get; set; }
    public int Horizon_Id { get; set; }
    public int Version_Number { get; set; }
    public string Status { get; set; } = "Draft"; // Draft, Pending, Published, Archived
    public DateTime Created_At { get; set; }
    public string Created_By { get; set; } = string.Empty;
    public int? Objective_Value { get; set; }
    public List<AssignmentDto> Assignments { get; set; } = new();
}

public class RosterDiffDto
{
    public int Version_A_Id { get; set; }
    public int Version_B_Id { get; set; }
    public List<AssignmentChangeDto> Changes { get; set; } = new();
}

public class RosterItemDto
{
    public int EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public string ShiftType { get; set; } = string.Empty;
}