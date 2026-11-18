using LearningTracker.Models.Entities;

namespace LearningTracker.Repositories.Interfaces;

public interface ILearnerRepository
{
    Task<Learner?> GetByIdAsync(
        Guid id);

    Task<Learner?> GetByAccountIdAsync(
        Guid accountId);

    Task<List<Learner>> GetAllAsync();

    Task AddAsync(
        Learner learner);

    Task SaveChangesAsync();

    Task<int> CountAsync();

    Task<List<Learner>> GetAllWithAssignmentsAsync();
}
