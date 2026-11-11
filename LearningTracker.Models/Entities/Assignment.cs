using System;
using LearningTracker.Models.Enums;

namespace LearningTracker.Models.Entities;

public class Assignment
{
    public Guid Id { get; set; }

    public Guid LearnerId { get; set; }

    public Guid ActivityId { get; set; }

    public AssignmentStatus Status { get; set; }
        = AssignmentStatus.NotStarted;

    // Navigation properties
    public Learner Learner { get; set; } = null!;

    public Activity Activity { get; set; } = null!;
}
