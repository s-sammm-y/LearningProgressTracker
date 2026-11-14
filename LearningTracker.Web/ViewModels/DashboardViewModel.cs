using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;

namespace LearningTracker.Web.ViewModels;

public class DashboardViewModel
{
    public int TotalLearners { get; set; }
    
    public int TotalActivities { get; set; }

    public int TotalAssignments { get; set; }

    public int CompletedAssignments { get; set; }

    public int InProgressAssignments { get; set; }

    public List<Activity> Activities { get; set; } = new();

    public List<CohortProgress> CohortProgress { get; set; } = new();
}