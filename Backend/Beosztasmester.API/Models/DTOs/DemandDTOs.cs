namespace Beosztasmester.Models.DTOs;

public class DemandDto
{
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    public DateTime Date { get; set; }
    public int ShiftTypeId { get; set; }
    public int? RequiredSkillId { get; set; }
    public int MinEmployees { get; set; }
    public int? MaxEmployees { get; set; }
}

public class CreateDemandDto
{
    public int DepartmentId { get; set; }
    public DateTime Date { get; set; }
    public int ShiftTypeId { get; set; }
    public int? RequiredSkillId { get; set; }
    public int MinEmployees { get; set; }
    public int? MaxEmployees { get; set; }
}