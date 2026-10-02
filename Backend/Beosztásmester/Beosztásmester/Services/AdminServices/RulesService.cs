using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Beosztasmester.Models;
using Beosztasmester.Models.Data;
using Beosztasmester.Models.DTOs;
using Beosztasmester.Model;

namespace Beosztasmester.Services;

public interface IRulesService
{
    Task<IEnumerable<RuleDto>> GetRulesAsync(Guid departmentId, bool? isActiveOnly);
    Task<RuleDto?> GetRuleByIdAsync(Guid id);
    Task<RuleDto> CreateRuleAsync(CreateRuleDto dto);
    Task<bool> UpdateRuleAsync(Guid id, CreateRuleDto dto);
    Task<bool> ToggleRuleStatusAsync(Guid id, bool isActive);
    Task<bool> DeleteRuleAsync(Guid id);

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
    public async Task<IEnumerable<RuleDto>> GetRulesAsync(Guid departmentId, bool? isActiveOnly)
    {
        var query = _dataContext.Rules.Where(r => r.Department_Id == departmentId);

        if (isActiveOnly.HasValue && isActiveOnly.Value)
        {
            query = query.Where(r => r.Is_Active);
        }

        return await query.Select(r => new RuleDto
        {
            Id = r.Id,
            DepartmentId = r.Department_Id,
            TypeCode = r.Type_Code,
            Scope = r.Scope,
            ScopeRefId = r.Scope_Ref_Id,
            IsHard = r.Is_Hard,
            Weight = r.Weight,
            ParametersJson = r.Parameters_Json,
            IsActive = r.Is_Active
        }).ToListAsync();
    }

    public async Task<RuleDto?> GetRuleByIdAsync(Guid id)
    {
        var rule = await _dataContext.Rules.FindAsync(id);
        if (rule == null) return null;

        return new RuleDto
        {
            Id = rule.Id,
            DepartmentId = rule.Department_Id,
            TypeCode = rule.Type_Code,
            Scope = rule.Scope,
            ScopeRefId = rule.Scope_Ref_Id,
            IsHard = rule.Is_Hard,
            Weight = rule.Weight,
            ParametersJson = rule.Parameters_Json,
            IsActive = rule.Is_Active
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
            Id = Guid.NewGuid(),
            Department_Id = dto.DepartmentId,
            Type_Code = dto.TypeCode,
            Scope = dto.Scope,
            Scope_Ref_Id = dto.ScopeRefId,
            Is_Hard = dto.IsHard,
            Weight = dto.Weight,
            Parameters_Json = dto.ParametersJson,
            Is_Active = true
        };

        _dataContext.Rules.Add(rule);
        await _dataContext.SaveChangesAsync();

        return new RuleDto
        {
            Id = rule.Id,
            DepartmentId = rule.Department_Id,
            TypeCode = rule.Type_Code,
            Scope = rule.Scope,
            ScopeRefId = rule.Scope_Ref_Id,
            IsHard = rule.Is_Hard,
            Weight = rule.Weight,
            ParametersJson = rule.Parameters_Json,
            IsActive = rule.Is_Active
        };
    }

    public async Task<bool> UpdateRuleAsync(Guid id, CreateRuleDto dto)
    {
        var rule = await _dataContext.Rules.FindAsync(id);
        if (rule == null) return false;

        if (!IsValidJson(dto.ParametersJson))
        {
            throw new ArgumentException("A megadott ParametersJson érvénytelen JSON formátum!");
        }

        rule.Type_Code = dto.TypeCode;
        rule.Scope = dto.Scope;
        rule.Scope_Ref_Id = dto.ScopeRefId;
        rule.Is_Hard = dto.IsHard;
        rule.Weight = dto.Weight;
        rule.Parameters_Json = dto.ParametersJson;

        await _dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleRuleStatusAsync(Guid id, bool isActive)
    {
        var rule = await _dataContext.Rules.FindAsync(id);
        if (rule == null) return false;

        rule.Is_Active = isActive;
        await _dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRuleAsync(Guid id)
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
