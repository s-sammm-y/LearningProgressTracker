using System;

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
}
