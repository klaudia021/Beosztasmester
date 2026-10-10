namespace Beosztasmester.Models.DTOs;

public class HorizonDto
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public DateTime StartDate { get; set; }
    public int Days { get; set; }
    public string Status { get; set; } = "Draft";
}

public class CreateHorizonRequestDto
{
    public Guid OrgUnitId { get; set; } // Vagy int, ha a frontendnél az orgUnitId egész szám
    public string Start { get; set; } = string.Empty; // "YYYY-MM-DD"
    public string End { get; set; } = string.Empty;   // "YYYY-MM-DD"
}

public class HorizonResponseDto
{
    public string Id { get; set; } = string.Empty;
    public Guid OrgUnitId { get; set; }
    public string Start { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
    public string Status { get; set; } = "DRAFT";
}


