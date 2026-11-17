using LearningTracker.Models.Enums;

namespace LearningTracker.Models.DTOs;

public class AssignmentDto
{
    public Guid AssignmentId { get; set; }

    public Guid LearnerId { get; set; }

    public Guid ActivityId { get; set; }

    public Guid LearnerAccountId { get; set; }

    public AssignmentStatus Status { get; set; }
}