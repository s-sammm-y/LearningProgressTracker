using System.Security.Cryptography;
using LearningTracker.Models.Entities;
using LearningTracker.Models.Enums;
using LearningTracker.Repositories.Interfaces;
using LearningTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LearningTracker.Services.Implementations;

public class LearnerService : ILearnerService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILearnerRepository _learnerRepository;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public LearnerService(
        IAccountRepository accountRepository,
        ILearnerRepository learnerRepository,
        IPasswordHasher<Account> passwordHasher)
    {
        _accountRepository = accountRepository;
        _learnerRepository = learnerRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<(Learner Learner, string TemporaryPassword)>
        CreateLearnerAsync(
            string displayName,
            string email,
            string cohort,
            CancellationToken cancellationToken = default)
    {
        // Basic input validation.
        if (string.IsNullOrWhiteSpace(displayName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(cohort))
        {
            throw new ArgumentException(
                "Name, email and cohort are required.");
        }

        // Normalize email.
        var normalizedEmail = email.Trim().ToUpperInvariant();

        // Check for duplicate email.
        var existingAccount =
            await _accountRepository.GetByNormalizedEmailAsync(
                normalizedEmail,
                cancellationToken);

        if (existingAccount is not null)
        {
            throw new InvalidOperationException(
                "An account with this email already exists.");
        }

        // Generate a secure temporary password.
        var temporaryPassword = GenerateTemporaryPassword();

        // Create account.
        var account = new Account
        {
            Id = Guid.NewGuid(),
            NormalizedEmail = normalizedEmail,
            Role = UserRole.Candidate,
            MustChangePassword = true,
            TemporaryPasswordExpiresAt = DateTime.UtcNow.AddHours(24),
            SecurityStamp = Guid.NewGuid().ToString()
        };

        // Store only the password hash.
        account.PasswordHash = _passwordHasher.HashPassword(
            account,
            temporaryPassword);

        // Create learner profile.
        var learner = new Learner
        {
            Id = Guid.NewGuid(),
            DisplayName = displayName.Trim(),
            Cohort = cohort.Trim(),
            AccountId = account.Id,
            Account = account
        };

        // Add both entities to the same DbContext.
        await _accountRepository.AddAsync(
            account,
            cancellationToken);

        await _learnerRepository.AddAsync(
            learner,
            cancellationToken);

        // Save both entities together.
        await _accountRepository.SaveChangesAsync(
            cancellationToken);

        // Return plaintext password only once.
        return (learner, temporaryPassword);
    }

    private static string GenerateTemporaryPassword()
    {
        const string uppercase = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string lowercase = "abcdefghijkmnopqrstuvwxyz";
        const string digits = "23456789";
        const string symbols = "!@#$%&*";

        const string allCharacters =
            uppercase + lowercase + digits + symbols;

        var passwordCharacters = new List<char>
        {
            uppercase[RandomNumberGenerator.GetInt32(uppercase.Length)],
            lowercase[RandomNumberGenerator.GetInt32(lowercase.Length)],
            digits[RandomNumberGenerator.GetInt32(digits.Length)],
            symbols[RandomNumberGenerator.GetInt32(symbols.Length)]
        };

        for (int i = 0; i < 12; i++)
        {
            passwordCharacters.Add(
                allCharacters[
                    RandomNumberGenerator.GetInt32(allCharacters.Length)]);
        }

        // Cryptographically shuffle the characters.
        for (int i = passwordCharacters.Count - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);

            (passwordCharacters[i], passwordCharacters[j]) =
                (passwordCharacters[j], passwordCharacters[i]);
        }

        return new string(passwordCharacters.ToArray());
    }

    public async Task<List<Learner>> GetAllLearnersAsync(
        CancellationToken cancellationToken = default)
    {
        return await _learnerRepository.GetAllAsync(
            cancellationToken);
    }
}
