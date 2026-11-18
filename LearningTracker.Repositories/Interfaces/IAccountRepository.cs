
using LearningTracker.Models.Entities;

namespace LearningTracker.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByNormalizedEmailAsync(
        string normalizedEmail);

    Task<Account?> GetByIdAsync(
        Guid id);

    Task AddAsync(
        Account account);

    Task SaveChangesAsync();
}
