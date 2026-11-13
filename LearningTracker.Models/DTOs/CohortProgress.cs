namespace LearningTracker.Models.DTOs;

public class CohortProgress
{
    public string CohortName { get; set; } = string.Empty;

    public int TotalLearners { get; set; }

    public int TotalAssignments { get; set; }

    public int CompletedAssignments { get; set; }

    public decimal CompletionPercentage { get; set; }
}

