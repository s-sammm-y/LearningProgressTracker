using LearningTracker.Models.DTOs;

namespace LearningTracker.Web.ViewModels;

public class DashboardViewModel
{
    public int TotalLearners { get; set; }

    public int TotalActivities { get; set; }

    public int TotalAssignments { get; set; }

    public int CompletedAssignments { get; set; }

    public List<CohortProgress> CohortProgress { get; set; } = new();
}