
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

    public async Task<int> GetTotalLearnersAsync()
    {
        return await _learnerRepository.CountAsync();
    }

    public async Task<int> GetTotalActivitiesAsync()
    {
        return await _activityRepository.CountAsync();
    }

    public async Task<int> GetTotalAssignmentsAsync()
    {
        return await _assignmentRepository.CountAsync();
    }

    public async Task<int> GetCompletedAssignmentsAsync()
    {
        return await _assignmentRepository.CountCompletedAsync();
    }

    public async Task<List<CohortProgress>> GetCohortProgressAsync()
    {
        var learners = await _learnerRepository
            .GetAllWithAssignmentsAsync();

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

    public async Task<int> GetInProgressAssignmentsAsync()
    {
        return await _assignmentRepository.CountInProgressAsync();
    }

    public async Task<List<Activity>> GetActivitiesAsync()
    {
        return await _activityRepository.GetAllAsync();
    }
}
