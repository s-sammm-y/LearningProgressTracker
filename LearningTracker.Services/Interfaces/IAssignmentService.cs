using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface IAssignmentService
{
    Task<Assignment> AssignActivityAsync(
        AssignmentDto dto);

    Task<bool> UpdateStatusAsync(
        AssignmentDto dto);

    Task<List<Assignment>> GetAssignmentsForLearnerAsync(
        Guid learnerAccountId);
}