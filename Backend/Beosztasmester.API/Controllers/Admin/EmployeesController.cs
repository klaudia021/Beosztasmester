using Microsoft.AspNetCore.Mvc;
using Beosztasmester.Services;
using Beosztasmester.Models.DTOs;

namespace Beosztasmester.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees( [FromQuery] int? departmentId, [FromQuery] string? status, [FromQuery] string? search)
        {
            var employees = await _employeeService.GetEmployeesAsync(departmentId, status, search);
            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            var result = await _employeeService.CreateEmployeeAsync(dto);
            return CreatedAtAction(nameof(GetEmployee), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
        {
            var updated = await _employeeService.UpdateEmployeeAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

    }
}
