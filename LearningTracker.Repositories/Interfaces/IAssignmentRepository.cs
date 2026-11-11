using LearningTracker.Models.Entities;

namespace LearningTracker.Repositories.Interfaces;

public interface IAssignmentRepository
{
    Task<Assignment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<Assignment>> GetByLearnerIdAsync(
        Guid learnerId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid learnerId,
        Guid activityId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Assignment assignment,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        CancellationToken cancellationToken = default);

    Task<int> CountCompletedAsync(
        CancellationToken cancellationToken = default);
}
