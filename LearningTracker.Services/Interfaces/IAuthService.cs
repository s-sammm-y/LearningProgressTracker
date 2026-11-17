using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface IAuthService
{
    Task<Account?> AuthenticateAsync(
        AuthDto dto,
        CancellationToken cancellationToken = default);

    Task<bool> ChangePasswordAsync(
        AuthDto dto,
        CancellationToken cancellationToken = default);
}