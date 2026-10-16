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
    public Guid OrgUnitId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}

public class HorizonResponseDto
{
    public string Id { get; set; } = string.Empty;
    public Guid OrgUnitId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Status { get; set; } = "DRAFT";
}


