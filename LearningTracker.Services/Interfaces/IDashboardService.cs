using System;
using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface IDashboardService
{
    Task<int> GetTotalLearnersAsync(
        CancellationToken cancellationToken = default);

    Task<int> GetTotalActivitiesAsync(
        CancellationToken cancellationToken = default);

    Task<int> GetTotalAssignmentsAsync(
        CancellationToken cancellationToken = default);

    Task<int> GetCompletedAssignmentsAsync(
        CancellationToken cancellationToken = default);

    Task<List<CohortProgress>> GetCohortProgressAsync(
        CancellationToken cancellationToken = default);
    
    Task<int> GetInProgressAssignmentsAsync(
        CancellationToken cancellationToken = default);

    Task<List<Activity>> GetActivitiesAsync(
        CancellationToken cancellationToken = default);
}
