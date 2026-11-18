
using LearningTracker.Models.DTOs;
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
        CoordinatorDto dto)
    {
        if (
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new ArgumentException(
                "Email and password are required.");
        }

        // Normalize email.
        var normalizedEmail = dto.Email.Trim().ToUpperInvariant();

        // Check whether the email is already registered.
        var existingAccount =
            await _accountRepository.GetByNormalizedEmailAsync(
                normalizedEmail);

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
            dto.Password);

        // Add account to the database.
        await _accountRepository.AddAsync(account);

        await _accountRepository.SaveChangesAsync();

        return account;
    }
}
