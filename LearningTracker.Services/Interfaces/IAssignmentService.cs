using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface IAssignmentService
{
    Task<Assignment> AssignActivityAsync(
        AssignmentDto dto,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateStatusAsync(
        AssignmentDto dto,
        CancellationToken cancellationToken = default);

    Task<List<Assignment>> GetAssignmentsForLearnerAsync(
        Guid learnerAccountId,
        CancellationToken cancellationToken = default);
}