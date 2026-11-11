using System;

namespace LearningTracker.Models.Entities;

public class Learner
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string Cohort { get; set; } = string.Empty;

    public Guid AccountId { get; set; }

    // Navigation properties
    public Account Account { get; set; } = null!;

    public ICollection<Assignment> Assignments { get; set; }
        = new List<Assignment>();
}
