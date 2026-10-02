namespace Beosztasmester.Models.DTOs;

public class StartSolveRequestDto
{
    public int TimeLimitSeconds { get; set; } = 300;
    public bool UseGreedyWarmStart { get; set; } = false;
}

public class SolverRunStatusDto
{
    public Guid RunId { get; set; }
    public Guid HorizonId { get; set; }
    public string Status { get; set; } = "Running"; // Running, Completed, Infeasible, Cancelled
    public double RuntimeSeconds { get; set; }
    public int? BestObjectiveValue { get; set; }
    public int? LowerBound { get; set; }
    public string? QualityMessage { get; set; }
}

public class SolverProgressDto
{
    public Guid RunId { get; set; }
    public double ElapsedSeconds { get; set; }
    public int CurrentObjective { get; set; }
    public int BestBound { get; set; }
}

public class InfeasibilityExplanationDto
{
    public Guid RunId { get; set; }
    public List<string> ConflictingRuleCodes { get; set; } = new();
    public List<string> HumanReadableReasons { get; set; } = new();
    public List<string> SuggestedFixes { get; set; } = new();
}