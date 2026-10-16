namespace Beosztasmester.Models.DTOs;

public class LoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthTokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserProfileDto User { get; set; } = null!;
}

public class UserProfileDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Employee, Planner, Admin
    public int? DepartmentId { get; set; }
    public int? EmployeeId { get; set; }
}

public class DelegatePlannerDto
{
    public int PlannerUserId { get; set; }
    public int DelegateUserId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}