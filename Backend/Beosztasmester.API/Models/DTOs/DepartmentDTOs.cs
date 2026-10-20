namespace Beosztasmester.Models.DTOs;

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
}

public class CreateDepartmentDto
{
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
}

