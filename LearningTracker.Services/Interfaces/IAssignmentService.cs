
using LearningTracker.Models.Entities;
using LearningTracker.Models.Enums;

namespace LearningTracker.Services.Interfaces;

public interface IAssignmentService
{
    Task<Assignment> AssignActivityAsync(
        Guid learnerId,
        Guid activityId,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateStatusAsync(
        Guid assignmentId,
        Guid learnerAccountId,
        AssignmentStatus status,
        CancellationToken cancellationToken = default);

    Task<List<Assignment>> GetAssignmentsForLearnerAsync(
        Guid learnerAccountId,
        CancellationToken cancellationToken = default);
}
