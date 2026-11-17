namespace LearningTracker.Models.DTOs;

public class ActivityDto
{
    public Guid ActivityId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Topic { get; set; } = string.Empty;

    public decimal PlannedHours { get; set; }
}