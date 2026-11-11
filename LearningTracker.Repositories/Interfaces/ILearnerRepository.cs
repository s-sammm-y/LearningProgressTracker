using LearningTracker.Models.Entities;

namespace LearningTracker.Repositories.Interfaces;

public interface ILearnerRepository
{
    Task<Learner?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Learner?> GetByAccountIdAsync(
        Guid accountId,
        CancellationToken cancellationToken = default);

    Task<List<Learner>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Learner learner,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
    CancellationToken cancellationToken = default);
}
