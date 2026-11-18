
using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;
using LearningTracker.Repositories.Interfaces;
using LearningTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LearningTracker.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AuthService(
        IAccountRepository accountRepository,
        IPasswordHasher<Account> passwordHasher)
    {
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Account?> AuthenticateAsync(AuthDto dto)
    {
        // Normalize email before searching.
        var normalizedEmail = dto.Email.Trim().ToUpperInvariant();

        var account = await _accountRepository.GetByNormalizedEmailAsync(
            normalizedEmail);

        if (account is null)
            return null;

        // Verify submitted password against the stored hash.
        var verificationResult = _passwordHasher.VerifyHashedPassword(
            account,
            account.PasswordHash,
            dto.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            return null;

        // Reject expired temporary credentials.
        if (account.MustChangePassword &&
            account.TemporaryPasswordExpiresAt.HasValue &&
            account.TemporaryPasswordExpiresAt.Value <= DateTime.UtcNow)
        {
            return null;
        }

        // Upgrade the password hash if the hasher recommends it.
        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            account.PasswordHash = _passwordHasher.HashPassword(
                account,
                dto.Password);

            await _accountRepository.SaveChangesAsync();
        }

        return account;
    }

    public async Task<bool> ChangePasswordAsync(AuthDto dto)
    {
        var account = await _accountRepository.GetByIdAsync(
            dto.AccountId);

        if (account is null)
            return false;

        // Verify the current password.
        var verificationResult = _passwordHasher.VerifyHashedPassword(
            account,
            account.PasswordHash,
            dto.CurrentPassword);

        if (verificationResult == PasswordVerificationResult.Failed)
            return false;

        // Do not allow an expired temporary password to be used.
        if (account.MustChangePassword &&
            account.TemporaryPasswordExpiresAt.HasValue &&
            account.TemporaryPasswordExpiresAt.Value <= DateTime.UtcNow)
        {
            return false;
        }

        // Hash and store the new password.
        account.PasswordHash = _passwordHasher.HashPassword(
            account,
            dto.NewPassword);

        // Remove temporary-password restrictions.
        account.MustChangePassword = false;
        account.TemporaryPasswordExpiresAt = null;

        // Invalidate existing authentication sessions.
        account.SecurityStamp = Guid.NewGuid().ToString();

        await _accountRepository.SaveChangesAsync();

        return true;
    }
}
