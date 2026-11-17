
using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;
using LearningTracker.Repositories.Interfaces;
using LearningTracker.Services.Interfaces;

namespace LearningTracker.Services.Implementations;

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;

    public ActivityService(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<Activity> CreateActivityAsync(
        ActivityDto dto,
        CancellationToken cancellationToken = default)
    {
        ValidateActivityDetails(
            dto.Title,
            dto.Topic,
            dto.PlannedHours);

        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            Title = dto.Title.Trim(),
            Topic = dto.Topic.Trim(),
            PlannedHours = dto.PlannedHours
        };

        await _activityRepository.AddAsync(
            activity,
            cancellationToken);

        await _activityRepository.SaveChangesAsync(
            cancellationToken);

        return activity;
    }

    public async Task<bool> UpdateActivityAsync(
        ActivityDto dto,
        CancellationToken cancellationToken = default)
    {
        ValidateActivityDetails(
            dto.Title,
            dto.Topic,
            dto.PlannedHours);

        var activity = await _activityRepository.GetByIdAsync(
            dto.ActivityId,
            cancellationToken);

        if (activity is null)
            return false;

        activity.Title = dto.Title.Trim();
        activity.Topic = dto.Topic.Trim();
        activity.PlannedHours = dto.PlannedHours;

        await _activityRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }

    private static void ValidateActivityDetails(
        string title,
        string topic,
        decimal plannedHours)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Activity title is required.");

        if (string.IsNullOrWhiteSpace(topic))
            throw new ArgumentException("Activity topic is required.");

        if (plannedHours <= 0)
            throw new ArgumentException(
                "Planned hours must be greater than zero.");
    }

    public async Task<List<Activity>> GetAllActivitiesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _activityRepository.GetAllAsync(
            cancellationToken);
    }

    public async Task<Activity?> GetActivityByIdAsync(
        Guid activityId,
        CancellationToken cancellationToken = default)
    {
        return await _activityRepository.GetByIdAsync(
            activityId,
            cancellationToken);
    }
}
