namespace Beosztasmester.Models.DTOs;

public class AssignmentDto
{
    public Guid Id { get; set; }
    public Guid RosterVersionId { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public Guid ShiftTypeId { get; set; }
    public bool IsLocked { get; set; }
}

public class PatchAssignmentDto
{
    public Guid EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public Guid? ShiftTypeId { get; set; } // null = törlés
    public bool IsLocked { get; set; }
}

public class AssignmentChangeDto
{
    public Guid EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public Guid? OldShiftTypeId { get; set; }
    public Guid? NewShiftTypeId { get; set; }
    public string ChangeType { get; set; } = "Modified"; // Added, Removed, Modified
}