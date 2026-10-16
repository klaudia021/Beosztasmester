namespace Beosztasmester.Models.DTOs;

public class AssignmentDto
{
    public int Id { get; set; }
    public int Roster_Version_Id { get; set; }
    public int Employee_Id { get; set; }
    public DateTime Date { get; set; }
    public int Shift_Type_Id { get; set; }
    public bool IsLocked { get; set; }
}

public class PatchAssignmentDto
{
    public int Employee_Id { get; set; }
    public DateTime Date { get; set; }
    public int? Shift_Type_Id { get; set; } // null = törlés
    public bool IsLocked { get; set; }
}

public class AssignmentChangeDto
{
    public int Employee_Id { get; set; }
    public DateTime Date { get; set; }
    public int? Old_Shift_Type_Id { get; set; }
    public int? New_Shift_Type_Id { get; set; }
    public string Change_Type { get; set; } = "Modified"; // Added, Removed, Modified
}