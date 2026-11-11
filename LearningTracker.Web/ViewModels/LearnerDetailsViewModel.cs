using LearningTracker.Models.Entities;

namespace LearningTracker.Web.ViewModels;

public class LearnerDetailsViewModel
{
    public Learner Learner { get; set; } = null!;

    public List<Assignment> Assignments { get; set; } = new();
}