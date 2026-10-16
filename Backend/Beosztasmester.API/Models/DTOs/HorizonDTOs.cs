namespace Beosztasmester.Models.DTOs;

public class HorizonDto
{
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    public DateTime StartDate { get; set; }
    public int Days { get; set; }
    public string Status { get; set; } = "Draft";
}

public class CreateHorizonRequestDto
{
    public int OrgUnitId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}

public class HorizonResponseDto
{
    public int Id { get; set; }
    public int OrgUnitId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Status { get; set; } = "DRAFT";
}

