using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface IActivityService
{
    Task<Activity> CreateActivityAsync(
        ActivityDto dto,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateActivityAsync(
        ActivityDto dto,
        CancellationToken cancellationToken = default);

    Task<List<Activity>> GetAllActivitiesAsync(
        CancellationToken cancellationToken = default);

    Task<Activity?> GetActivityByIdAsync(
        Guid activityId,
        CancellationToken cancellationToken = default);

    Task<byte[]> ExportActivitiesCsvAsync(
        CancellationToken cancellationToken = default);

    Task<ActivityCsvImportResultDto> ImportActivitiesCsvAsync(
        Stream csvStream,
        CancellationToken cancellationToken = default);
}