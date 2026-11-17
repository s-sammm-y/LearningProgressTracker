namespace LearningTracker.Models.DTOs;

public class LearnerDto
{
    public Guid LearnerId { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Cohort { get; set; } = string.Empty;
}