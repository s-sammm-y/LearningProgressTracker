
using LearningTracker.Models.Entities;
using LearningTracker.Models.Enums;
using LearningTracker.Repositories.Interfaces;
using LearningTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LearningTracker.Services.Implementations;

public class CoordinatorService : ICoordinatorService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public CoordinatorService(
        IAccountRepository accountRepository,
        IPasswordHasher<Account> passwordHasher)
    {
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Account> CreateCoordinatorAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException(
                "Name, email and password are required.");
        }

        // Normalize email.
        var normalizedEmail = email.Trim().ToUpperInvariant();

        // Check whether the email is already registered.
        var existingAccount =
            await _accountRepository.GetByNormalizedEmailAsync(
                normalizedEmail,
                cancellationToken);

        if (existingAccount is not null)
        {
            throw new InvalidOperationException(
                "An account with this email already exists.");
        }

        // Create coordinator account.
        var account = new Account
        {
            Id = Guid.NewGuid(),
            NormalizedEmail = normalizedEmail,
            Role = UserRole.Coordinator,
            MustChangePassword = false,
            TemporaryPasswordExpiresAt = null,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        // Hash the coordinator's password.
        account.PasswordHash = _passwordHasher.HashPassword(
            account,
            password);

        // Add account to the database.
        await _accountRepository.AddAsync(
            account,
            cancellationToken);

        await _accountRepository.SaveChangesAsync(
            cancellationToken);

        return account;
    }
}
