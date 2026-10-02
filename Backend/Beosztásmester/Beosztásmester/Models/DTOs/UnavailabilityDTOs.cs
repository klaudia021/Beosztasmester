namespace Beosztasmester.Models.DTOs;

public class UnavailabilityDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string Type { get; set; } = string.Empty; // Vacation, Sick, Training
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class CreateUnavailabilityDto
{
    public Guid EmployeeId { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
}