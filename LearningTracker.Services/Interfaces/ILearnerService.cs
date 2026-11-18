using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface ILearnerService
{
    Task<(Learner Learner, string TemporaryPassword)> CreateLearnerAsync(
        LearnerDto dto);

    Task<List<Learner>> GetAllLearnersAsync();

    Task<Learner?> GetLearnerByIdAsync(
        Guid learnerId);

    Task<string?> ResetTemporaryPasswordAsync(
        Guid learnerId);
}