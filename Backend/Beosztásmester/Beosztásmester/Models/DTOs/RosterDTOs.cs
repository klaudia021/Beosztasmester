namespace Beosztasmester.Models.DTOs;

public class RosterVersionDto
{
    public Guid Id { get; set; }
    public Guid Horizon_Id { get; set; }
    public int Version_Number { get; set; }
    public string Status { get; set; } = "Draft"; // Draft, Pending, Published, Archived
    public DateTime Created_At { get; set; }
    public string Created_By { get; set; } = string.Empty;
    public int? Objective_Value { get; set; }
    public List<AssignmentDto> Assignments { get; set; } = new();
}

public class RosterDiffDto
{
    public Guid Version_A_Id { get; set; }
    public Guid Version_B_Id { get; set; }
    public List<AssignmentChangeDto> Changes { get; set; } = new();
}

public class RosterItemDto
{
    public string Employee_Id { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Shift_Type { get; set; } = string.Empty;
}