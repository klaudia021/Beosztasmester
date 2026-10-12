namespace Beosztasmester.Models.DTOs;

public class AssignmentDto
{
    public Guid Id { get; set; }
    public Guid Roster_Version_Id { get; set; }
    public Guid Employee_Id { get; set; }
    public DateTime Date { get; set; }
    public Guid Shift_Type_Id { get; set; }
    public bool IsLocked { get; set; }
}

public class PatchAssignmentDto
{
    public Guid Employee_Id { get; set; }
    public DateTime Date { get; set; }
    public Guid? Shift_Type_Id { get; set; } // null = törlés
    public bool IsLocked { get; set; }
}

public class AssignmentChangeDto
{
    public Guid Employee_Id { get; set; }
    public DateTime Date { get; set; }
    public Guid? Old_Shift_Type_Id { get; set; }
    public Guid? New_Shift_Type_Id { get; set; }
    public string Change_Type { get; set; } = "Modified"; // Added, Removed, Modified
}