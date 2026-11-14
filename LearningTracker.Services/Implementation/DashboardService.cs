
using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;
using LearningTracker.Models.Enums;
using LearningTracker.Repositories.Interfaces;
using LearningTracker.Services.Interfaces;

namespace LearningTracker.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly ILearnerRepository _learnerRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IAssignmentRepository _assignmentRepository;

    public DashboardService(
        ILearnerRepository learnerRepository,
        IActivityRepository activityRepository,
        IAssignmentRepository assignmentRepository)
    {
        _learnerRepository = learnerRepository;
        _activityRepository = activityRepository;
        _assignmentRepository = assignmentRepository;
    }

    public async Task<int> GetTotalLearnersAsync(
        CancellationToken cancellationToken = default)
    {
        return await _learnerRepository.CountAsync(cancellationToken);
    }

    public async Task<int> GetTotalActivitiesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _activityRepository.CountAsync(cancellationToken);
    }

    public async Task<int> GetTotalAssignmentsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _assignmentRepository.CountAsync(cancellationToken);
    }

    public async Task<int> GetCompletedAssignmentsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _assignmentRepository.CountCompletedAsync(
            cancellationToken);
    }

    public async Task<List<CohortProgress>> GetCohortProgressAsync(
        CancellationToken cancellationToken = default)
    {
        var learners = await _learnerRepository
            .GetAllWithAssignmentsAsync(cancellationToken);

        return learners
            .GroupBy(l => l.Cohort)
            .Select(group =>
            {
                var totalAssignments = group.Sum(l => l.Assignments.Count);

                var completedAssignments = group.Sum(l =>
                    l.Assignments.Count(a =>
                        a.Status == AssignmentStatus.Completed));

                return new CohortProgress
                {
                    CohortName = group.Key,
                    TotalLearners = group.Count(),
                    TotalAssignments = totalAssignments,
                    CompletedAssignments = completedAssignments,

                    CompletionPercentage = totalAssignments == 0
                        ? 0
                        : Math.Round(
                            (decimal)completedAssignments / totalAssignments * 100,
                            2)
                };
            })
            .OrderBy(p => p.CohortName)
            .ToList();
    }

    public async Task<int> GetInProgressAssignmentsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _assignmentRepository.CountInProgressAsync(
            cancellationToken);
    }

    public async Task<List<Activity>> GetActivitiesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _activityRepository.GetAllAsync(cancellationToken);
    }
}
