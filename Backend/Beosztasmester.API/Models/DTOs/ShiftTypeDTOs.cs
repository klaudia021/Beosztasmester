namespace Beosztasmester.Models.DTOs;

public class ShiftTypeDto
{
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty; // HH:mm
    public string EndTime { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public bool IsNight { get; set; }
}

public class CreateShiftTypeDto
{
    public int DepartmentId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public bool IsNight { get; set; }
}