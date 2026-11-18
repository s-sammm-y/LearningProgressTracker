using LearningTracker.Models.Entities;

namespace LearningTracker.Repositories.Interfaces;

public interface IAssignmentRepository
{
    Task<Assignment?> GetByIdAsync(
        Guid id);

    Task<List<Assignment>> GetByLearnerIdAsync(
        Guid learnerId);

    Task<bool> ExistsAsync(
        Guid learnerId,
        Guid activityId);

    Task AddAsync(
        Assignment assignment);

    Task SaveChangesAsync();

    Task<int> CountAsync();

    Task<int> CountCompletedAsync();

    Task<int> CountInProgressAsync();
}
