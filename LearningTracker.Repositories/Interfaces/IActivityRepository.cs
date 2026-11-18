
using LearningTracker.Models.Entities;

namespace LearningTracker.Repositories.Interfaces;

public interface IActivityRepository
{
    Task<Activity?> GetByIdAsync(
        Guid id);

    Task<List<Activity>> GetAllAsync();

    Task AddAsync(
        Activity activity);

    Task SaveChangesAsync();

    Task<int> CountAsync();

    Task AddRangeAsync(
        IEnumerable<Activity> activities);
}
