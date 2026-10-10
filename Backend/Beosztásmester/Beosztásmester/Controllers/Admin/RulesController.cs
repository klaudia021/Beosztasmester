using Beosztasmester.DTOs;
using Beosztasmester.Models.DTOs;
using Beosztasmester.Services;
using Microsoft.AspNetCore.Mvc;

namespace Beosztasmester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly IRulesService _rulesService;

        public RulesController(IRulesService rulesService)
        {
            _rulesService = rulesService;
        }


        /// <summary>
        /// Egy szervezeti egység szabályainak lekérdezése
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RuleDto>>> GetRules(
            [FromQuery] Guid departmentId,
            [FromQuery] bool? isActiveOnly)
        {
            if (departmentId == Guid.Empty)
            {
                return BadRequest(new { message = "DepartmentId kötelező paraméter!" });
            }

            var rules = await _rulesService.GetRulesAsync(departmentId, isActiveOnly);
            return Ok(rules);
        }

        /// <summary>
        /// Egy konkret szabály lekérdezése ID alapján
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RuleDto>> GetRule(Guid id)
        {
            var rule = await _rulesService.GetRuleByIdAsync(id);
            if (rule == null) return NotFound();

            return Ok(rule);
        }

        /// <summary>
        /// Új szabály rögzítése a katalógusba (F3)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RuleDto>> CreateRule([FromBody] CreateRuleDto dto)
        {
            try
            {
                var result = await _rulesService.CreateRuleAsync(dto);
                return CreatedAtAction(nameof(GetRule), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Meglévő szabály módosítása
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRule(Guid id, [FromBody] CreateRuleDto dto)
        {
            try
            {
                var updated = await _rulesService.UpdateRuleAsync(id, dto);
                if (!updated) return NotFound();

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Szabály ki- vagy bekapcsolása (Aktiválás / Inaktiválás)
        /// </summary>
        [HttpPatch("{id:guid}/toggle")]
        public async Task<IActionResult> ToggleRuleStatus(Guid id, [FromQuery] bool isActive)
        {
            var updated = await _rulesService.ToggleRuleStatusAsync(id, isActive);
            if (!updated) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Szabály törlése
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRule(Guid id)
        {
            var deleted = await _rulesService.DeleteRuleAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// F10: Természetes nyelvű mondat feldolgozása AI segítségével
        /// </summary>
        [HttpPost("parse")]
        public async Task<ActionResult<ParseRuleResponseDto>> ParseNaturalLanguageRule([FromBody] ParseRuleRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NaturalText))
            {
                return BadRequest(new { message = "A természetes nyelvű szöveg nem lehet üres!" });
            }

            var result = await _rulesService.ParseNaturalLanguageRuleAsync(dto);
            return Ok(result);
        }

    }
}
