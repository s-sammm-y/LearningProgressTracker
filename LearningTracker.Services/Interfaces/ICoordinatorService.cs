using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface ICoordinatorService
{
    Task<Account> CreateCoordinatorAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}