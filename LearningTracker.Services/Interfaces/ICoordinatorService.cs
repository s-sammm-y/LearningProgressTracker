
using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface ICoordinatorService
{
    Task<Account> CreateCoordinatorAsync(
        CoordinatorDto dto);
}
