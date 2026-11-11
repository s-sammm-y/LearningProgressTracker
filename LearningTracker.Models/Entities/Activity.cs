using System;

namespace LearningTracker.Models.Entities;

public class Activity
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Topic { get; set; } = string.Empty;

    public decimal PlannedHours { get; set; }

    // Navigation property
    public ICollection<Assignment> Assignments { get; set; }
        = new List<Assignment>();
}
