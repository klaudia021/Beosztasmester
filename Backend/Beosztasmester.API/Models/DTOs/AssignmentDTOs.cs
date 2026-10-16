namespace Beosztasmester.Models.DTOs;

public class AssignmentDto
{
    public int Id { get; set; }
    public int RosterVersionId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public int ShiftTypeId { get; set; }
    public bool IsLocked { get; set; }
}

public class PatchAssignmentDto
{
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public int? ShiftTypeId { get; set; } // null = törlés
    public bool IsLocked { get; set; }
}

public class AssignmentChangeDto
{
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public int? OldShiftTypeId { get; set; }
    public int? NewShiftTypeId { get; set; }
    public string ChangeType { get; set; } = "Modified"; // Added, Removed, Modified
}