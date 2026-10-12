namespace Beosztasmester.Models.DTOs;

public class DemandDto
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public DateTime Date { get; set; }
    public Guid ShiftTypeId { get; set; }
    public Guid? RequiredSkillId { get; set; }
    public int MinEmployees { get; set; }
    public int? MaxEmployees { get; set; }
}

public class CreateDemandDto
{
    public Guid DepartmentId { get; set; }
    public DateTime Date { get; set; }
    public Guid ShiftTypeId { get; set; }
    public Guid? RequiredSkillId { get; set; }
    public int MinEmployees { get; set; }
    public int? MaxEmployees { get; set; }
}