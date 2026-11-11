
using LearningTracker.Models.Entities;
using LearningTracker.Models.Enums;
using LearningTracker.Repositories.Interfaces;
using LearningTracker.Services.Interfaces;

namespace LearningTracker.Services.Implementations;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ILearnerRepository _learnerRepository;
    private readonly IActivityRepository _activityRepository;

    public AssignmentService(
        IAssignmentRepository assignmentRepository,
        ILearnerRepository learnerRepository,
        IActivityRepository activityRepository)
    {
        _assignmentRepository = assignmentRepository;
        _learnerRepository = learnerRepository;
        _activityRepository = activityRepository;
    }

    public async Task<Assignment> AssignActivityAsync(
        Guid learnerId,
        Guid activityId,
        CancellationToken cancellationToken = default)
    {
        var learner = await _learnerRepository.GetByIdAsync(
            learnerId,
            cancellationToken);

        if (learner is null)
            throw new InvalidOperationException("Learner not found.");

        var activity = await _activityRepository.GetByIdAsync(
            activityId,
            cancellationToken);

        if (activity is null)
            throw new InvalidOperationException("Activity not found.");

        var alreadyAssigned = await _assignmentRepository.ExistsAsync(
            learnerId,
            activityId,
            cancellationToken);

        if (alreadyAssigned)
            throw new InvalidOperationException(
                "This activity is already assigned to the learner.");

        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            LearnerId = learnerId,
            ActivityId = activityId,
            Status = AssignmentStatus.NotStarted
        };

        await _assignmentRepository.AddAsync(
            assignment,
            cancellationToken);

        await _assignmentRepository.SaveChangesAsync(
            cancellationToken);

        return assignment;
    }

    public async Task<bool> UpdateStatusAsync(
        Guid assignmentId,
        Guid learnerAccountId,
        AssignmentStatus status,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(typeof(AssignmentStatus), status))
            throw new ArgumentException("Invalid assignment status.");

        var learner = await _learnerRepository.GetByAccountIdAsync(
            learnerAccountId,
            cancellationToken);

        if (learner is null)
            return false;

        var assignment = await _assignmentRepository.GetByIdAsync(
            assignmentId,
            cancellationToken);

        if (assignment is null || assignment.LearnerId != learner.Id)
            return false;

        assignment.Status = status;

        await _assignmentRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }

    public async Task<List<Assignment>> GetAssignmentsForLearnerAsync(
        Guid learnerAccountId,
        CancellationToken cancellationToken = default)
    {
        var learner = await _learnerRepository.GetByAccountIdAsync(
            learnerAccountId,
            cancellationToken);

        if (learner is null)
            return new List<Assignment>();

        return await _assignmentRepository.GetByLearnerIdAsync(
            learner.Id,
            cancellationToken);
    }
}
