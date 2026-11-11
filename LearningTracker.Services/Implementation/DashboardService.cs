
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
}
