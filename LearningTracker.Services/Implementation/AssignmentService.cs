
using LearningTracker.Models.DTOs;
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

    public async Task<Assignment> AssignActivityAsync(AssignmentDto dto)
    {
        var learner = await _learnerRepository.GetByIdAsync(
            dto.LearnerId);

        if (learner is null)
            throw new InvalidOperationException("Learner not found.");

        var activity = await _activityRepository.GetByIdAsync(
            dto.ActivityId);

        if (activity is null)
            throw new InvalidOperationException("Activity not found.");

        var alreadyAssigned = await _assignmentRepository.ExistsAsync(
            dto.LearnerId,
            dto.ActivityId);

        if (alreadyAssigned)
            throw new InvalidOperationException(
                "This activity is already assigned to the learner.");

        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            LearnerId = dto.LearnerId,
            ActivityId = dto.ActivityId,
            Status = AssignmentStatus.NotStarted
        };

        await _assignmentRepository.AddAsync(assignment);

        await _assignmentRepository.SaveChangesAsync();

        return assignment;
    }

    public async Task<bool> UpdateStatusAsync(AssignmentDto dto)
    {
        if (!Enum.IsDefined(typeof(AssignmentStatus), dto.Status))
            throw new ArgumentException("Invalid assignment status.");

        var learner = await _learnerRepository.GetByAccountIdAsync(
            dto.LearnerAccountId);

        if (learner is null)
            return false;

        var assignment = await _assignmentRepository.GetByIdAsync(
            dto.AssignmentId);

        if (assignment is null || assignment.LearnerId != learner.Id)
            return false;

        assignment.Status = dto.Status;

        await _assignmentRepository.SaveChangesAsync();

        return true;
    }

    public async Task<List<Assignment>> GetAssignmentsForLearnerAsync(
        Guid learnerAccountId)
    {
        var learner = await _learnerRepository.GetByAccountIdAsync(
            learnerAccountId);

        if (learner is null)
            return new List<Assignment>();

        return await _assignmentRepository.GetByLearnerIdAsync(
            learner.Id);
    }
}
