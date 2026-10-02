using Beosztasmester.Models.DTOs;
using Beosztasmester.Services;
using Microsoft.AspNetCore.Mvc;

namespace Beosztasmester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : Controller
    {
        private readonly IMasterDataService _masterDataService;

        public MasterDataController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        // ==========================================
        // DEPARTMENTS (Szervezeti Egységek)
        // ==========================================
        [HttpGet("departments")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var departments = await _masterDataService.GetDepartmentsAsync();
            return Ok(departments);
        }

        [HttpGet("departments/{id:guid}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(Guid id)
        {
            var department = await _masterDataService.GetDepartmentByIdAsync(id);
            if (department == null) return NotFound();

            return Ok(department);
        }

        [HttpPost("departments")]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] CreateDepartmentDto dto)
        {
            var result = await _masterDataService.CreateDepartmentAsync(dto);
            return CreatedAtAction(nameof(GetDepartment), new { id = result.Id }, result);
        }

        // ==========================================
        // SKILLS (Kompetenciák)
        // ==========================================
        [HttpGet("skills")]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetSkills()
        {
            var skills = await _masterDataService.GetSkillsAsync();
            return Ok(skills);
        }

        [HttpGet("skills/{id:guid}")]
        public async Task<ActionResult<SkillDto>> GetSkill(Guid id)
        {
            var skill = await _masterDataService.GetSkillByIdAsync(id);
            if (skill == null) return NotFound();

            return Ok(skill);
        }

        [HttpPost("skills")]
        public async Task<ActionResult<SkillDto>> CreateSkill([FromBody] CreateSkillDto dto)
        {
            var result = await _masterDataService.CreateSkillAsync(dto);
            return CreatedAtAction(nameof(GetSkill), new { id = result.Id }, result);
        }

        // ==========================================
        // SHIFT TYPES (Műszaktípusok)
        // ==========================================
        [HttpGet("shift-types")]
        public async Task<ActionResult<IEnumerable<ShiftTypeDto>>> GetShiftTypes([FromQuery] Guid? departmentId)
        {
            var shiftTypes = await _masterDataService.GetShiftTypesAsync(departmentId);
            return Ok(shiftTypes);
        }

        [HttpGet("shift-types/{id:guid}")]
        public async Task<ActionResult<ShiftTypeDto>> GetShiftType(Guid id)
        {
            var shiftType = await _masterDataService.GetShiftTypeByIdAsync(id);
            if (shiftType == null) return NotFound();

            return Ok(shiftType);
        }

        [HttpPost("shift-types")]
        public async Task<ActionResult<ShiftTypeDto>> CreateShiftType([FromBody] CreateShiftTypeDto dto)
        {
            var result = await _masterDataService.CreateShiftTypeAsync(dto);
            return CreatedAtAction(nameof(GetShiftType), new { id = result.Id }, result);
        }

    }
}
