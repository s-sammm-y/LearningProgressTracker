using System;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface IAuthService
{
    Task<Account?> AuthenticateAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<bool> ChangePasswordAsync(
        Guid accountId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default);
}
