using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface IActivityService
{
    Task<Activity> CreateActivityAsync(
        ActivityDto dto);

    Task<bool> UpdateActivityAsync(
        ActivityDto dto);

    Task<List<Activity>> GetAllActivitiesAsync();

    Task<Activity?> GetActivityByIdAsync(
        Guid activityId);

    Task<byte[]> ExportActivitiesCsvAsync();

    Task<ActivityCsvImportResultDto> ImportActivitiesCsvAsync(
        Stream csvStream);
}