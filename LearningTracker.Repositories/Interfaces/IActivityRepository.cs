
using LearningTracker.Models.Entities;

namespace LearningTracker.Repositories.Interfaces;

public interface IActivityRepository
{
    Task<Activity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<Activity>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Activity activity,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        CancellationToken cancellationToken = default);
}
