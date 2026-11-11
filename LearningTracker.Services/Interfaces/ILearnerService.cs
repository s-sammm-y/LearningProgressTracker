using System;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface ILearnerService
{
    Task<(Learner Learner, string TemporaryPassword)> CreateLearnerAsync(
        string displayName,
        string email,
        string cohort,
        CancellationToken cancellationToken = default);

    Task<List<Learner>> GetAllLearnersAsync(
        CancellationToken cancellationToken = default); 

    Task<Learner?> GetLearnerByIdAsync(
        Guid learnerId,
        CancellationToken cancellationToken = default);

    Task<string?> ResetTemporaryPasswordAsync(
        Guid learnerId,
        CancellationToken cancellationToken = default);
}
