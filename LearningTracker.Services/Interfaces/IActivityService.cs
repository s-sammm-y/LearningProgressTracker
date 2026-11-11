using System;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface IActivityService
{
    Task<Activity> CreateActivityAsync(
        string title,
        string topic,
        decimal plannedHours,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateActivityAsync(
        Guid activityId,
        string title,
        string topic,
        decimal plannedHours,
        CancellationToken cancellationToken = default);

    Task<List<Activity>> GetAllActivitiesAsync(
        CancellationToken cancellationToken = default);

    Task<Activity?> GetActivityByIdAsync(
        Guid activityId,
        CancellationToken cancellationToken = default);
}
