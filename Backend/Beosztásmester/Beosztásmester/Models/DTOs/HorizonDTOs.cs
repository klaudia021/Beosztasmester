namespace Beosztasmester.Models.DTOs;

public class HorizonDto
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public DateTime StartDate { get; set; }
    public int Days { get; set; }
    public string Status { get; set; } = "Draft";
}

public class CreateHorizonDto
{
    public Guid DepartmentId { get; set; }
    public DateTime StartDate { get; set; }
    public int Days { get; set; } = 28;
}