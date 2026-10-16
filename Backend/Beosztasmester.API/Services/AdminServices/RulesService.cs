using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;

namespace Beosztasmester.Services;

public interface IRulesService
{
    Task<IEnumerable<RuleDto>> GetRulesAsync(int departmentId, bool? isActiveOnly);
    Task<RuleDto?> GetRuleByIdAsync(int id);
    Task<RuleDto> CreateRuleAsync(CreateRuleDto dto);
    Task<bool> UpdateRuleAsync(int id, CreateRuleDto dto);
    Task<bool> ToggleRuleStatusAsync(int id, bool isActive);
    Task<bool> DeleteRuleAsync(int id);

    // F10: Természetes nyelvű szabály feldolgozása (AI / Mocked LLM)
    Task<ParseRuleResponseDto> ParseNaturalLanguageRuleAsync(ParseRuleRequestDto dto);
}
public class RulesService : IRulesService
{
    private readonly DataContext _dataContext;

    public RulesService(DataContext c)
    {
        _dataContext = c;
    }
    public async Task<IEnumerable<RuleDto>> GetRulesAsync(int departmentId, bool? isActiveOnly)
    {
        var query = _dataContext.Rules.Where(r => r.DepartmentId == departmentId);

        if (isActiveOnly.HasValue && isActiveOnly.Value)
        {
            query = query.Where(r => r.IsActive);
        }

        return await query.Select(r => new RuleDto
        {
            Id = r.Id,
            DepartmentId = r.DepartmentId,
            TypeCode = r.TypeCode,
            Scope = r.Scope,
            ScopeRefId = r.ScopeRefId,
            IsHard = r.IsHard,
            Weight = r.Weight,
            ParametersJson = r.ParametersJson,
            IsActive = r.IsActive
        }).ToListAsync();
    }

    public async Task<RuleDto?> GetRuleByIdAsync(int id)
    {
        var rule = await _dataContext.Rules.FindAsync(id);
        if (rule == null) return null;

        return new RuleDto
        {
            Id = rule.Id,
            DepartmentId = rule.DepartmentId,
            TypeCode = rule.TypeCode,
            Scope = rule.Scope,
            ScopeRefId = rule.ScopeRefId,
            IsHard = rule.IsHard,
            Weight = rule.Weight,
            ParametersJson = rule.ParametersJson,
            IsActive = rule.IsActive
        };
    }

    public async Task<RuleDto> CreateRuleAsync(CreateRuleDto dto)
    {
        // Validáljuk a JSON formátumot
        if (!IsValidJson(dto.ParametersJson))
        {
            throw new ArgumentException("A megadott ParametersJson érvénytelen JSON formátum!");
        }

        var rule = new Rule
        {
            DepartmentId = dto.DepartmentId,
            TypeCode = dto.TypeCode,
            Scope = dto.Scope,
            ScopeRefId = dto.ScopeRefId,
            IsHard = dto.IsHard,
            Weight = dto.Weight,
            ParametersJson = dto.ParametersJson,
            IsActive = true
        };

        _dataContext.Rules.Add(rule);
        await _dataContext.SaveChangesAsync();

        return new RuleDto
        {
            Id = rule.Id,
            DepartmentId = rule.DepartmentId,
            TypeCode = rule.TypeCode,
            Scope = rule.Scope,
            ScopeRefId = rule.ScopeRefId,
            IsHard = rule.IsHard,
            Weight = rule.Weight,
            ParametersJson = rule.ParametersJson,
            IsActive = rule.IsActive
        };
    }

    public async Task<bool> UpdateRuleAsync(int id, CreateRuleDto dto)
    {
        var rule = await _dataContext.Rules.FindAsync(id);
        if (rule == null) return false;

        if (!IsValidJson(dto.ParametersJson))
        {
            throw new ArgumentException("A megadott ParametersJson érvénytelen JSON formátum!");
        }

        rule.TypeCode = dto.TypeCode;
        rule.Scope = dto.Scope;
        rule.ScopeRefId = dto.ScopeRefId;
        rule.IsHard = dto.IsHard;
        rule.Weight = dto.Weight;
        rule.ParametersJson = dto.ParametersJson;

        await _dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleRuleStatusAsync(int id, bool isActive)
    {
        var rule = await _dataContext.Rules.FindAsync(id);
        if (rule == null) return false;

        rule.IsActive = isActive;
        await _dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRuleAsync(int id)
    {
        var rule = await _dataContext.Rules.FindAsync(id);
        if (rule == null) return false;

        _dataContext.Rules.Remove(rule);
        await _dataContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// F10: Szabadszöveges szabály értelmezése (NL Parsing).
    /// </summary>
    public async Task<ParseRuleResponseDto> ParseNaturalLanguageRuleAsync(ParseRuleRequestDto dto)
    {
        var text = dto.NaturalText.ToLower().Trim();

        // Példa kétértelműségi vizsgálatra (Specifikáció elvárás!)
        if (text.Contains("szerdánként nem dolgozhat") && !text.Contains("kovács") && !text.Contains("péter"))
        {
            return new ParseRuleResponseDto
            {
                NeedsClarification = true,
                ClarificationQuestion = "Melyik dolgozóra vonatkozik a szabály? Kérlek add meg a nevét!",
                HumanReadableDescription = "Hiányos kifejezés",
                ExtractedRule = null
            };
        }

        // Példa minta-felismerésre (Ezt később lecserélheted OpenAI / LLM API hívásra)
        if (text.Contains("éjszakai") && text.Contains("legfeljebb"))
        {
            return new ParseRuleResponseDto
            {
                NeedsClarification = false,
                HumanReadableDescription = "Egymást követő éjszakai műszakok maximális száma: 3 nap (Kemény korlátozás)",
                ExtractedRule = new CreateRuleDto
                {
                    DepartmentId = dto.DepartmentId,
                    TypeCode = "MAX_CONSECUTIVE_NIGHTS",
                    Scope = "Global",
                    IsHard = true,
                    Weight = 0,
                    ParametersJson = JsonSerializer.Serialize(new { max = 3 })
                }
            };
        }

        // Fallback / Alapértelmezett értelmezés
        return new ParseRuleResponseDto
        {
            NeedsClarification = false,
            HumanReadableDescription = $"Értelmezett szabály: {dto.NaturalText}",
            ExtractedRule = new CreateRuleDto
            {
                DepartmentId = dto.DepartmentId,
                TypeCode = "CUSTOM_RULE",
                Scope = "Global",
                IsHard = false,
                Weight = 5,
                ParametersJson = "{}"
            }
        };
    }

    private static bool IsValidJson(string strInput)
    {
        if (string.IsNullOrWhiteSpace(strInput)) return false;
        try
        {
            using var jsonDoc = JsonDocument.Parse(strInput);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
