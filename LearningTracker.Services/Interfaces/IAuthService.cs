using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface IAuthService
{
    Task<Account?> AuthenticateAsync(
        AuthDto dto);

    Task<bool> ChangePasswordAsync(
        AuthDto dto);
}