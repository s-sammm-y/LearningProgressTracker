
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
        string title,
        string topic,
        decimal plannedHours,
        CancellationToken cancellationToken = default)
    {
        ValidateActivityDetails(title, topic, plannedHours);

        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Topic = topic.Trim(),
            PlannedHours = plannedHours
        };

        await _activityRepository.AddAsync(
            activity,
            cancellationToken);

        await _activityRepository.SaveChangesAsync(
            cancellationToken);

        return activity;
    }

    public async Task<bool> UpdateActivityAsync(
        Guid activityId,
        string title,
        string topic,
        decimal plannedHours,
        CancellationToken cancellationToken = default)
    {
        ValidateActivityDetails(title, topic, plannedHours);

        var activity = await _activityRepository.GetByIdAsync(
            activityId,
            cancellationToken);

        if (activity is null)
            return false;

        activity.Title = title.Trim();
        activity.Topic = topic.Trim();
        activity.PlannedHours = plannedHours;

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
