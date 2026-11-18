
using System.Globalization;
using CsvHelper;
using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;
using LearningTracker.Repositories.Interfaces;
using LearningTracker.Services.Helpers;
using LearningTracker.Services.Interfaces;

namespace LearningTracker.Services.Implementations;

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;

    public ActivityService(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<Activity> CreateActivityAsync(ActivityDto dto)
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

        await _activityRepository.AddAsync(activity);

        await _activityRepository.SaveChangesAsync();

        return activity;
    }

    public async Task<bool> UpdateActivityAsync(ActivityDto dto)
    {
        ValidateActivityDetails(
            dto.Title,
            dto.Topic,
            dto.PlannedHours);

        var activity = await _activityRepository.GetByIdAsync(
            dto.ActivityId);

        if (activity is null)
            return false;

        activity.Title = dto.Title.Trim();
        activity.Topic = dto.Topic.Trim();
        activity.PlannedHours = dto.PlannedHours;

        await _activityRepository.SaveChangesAsync();

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

    public async Task<List<Activity>> GetAllActivitiesAsync()
    {
        return await _activityRepository.GetAllAsync();
    }

    public async Task<Activity?> GetActivityByIdAsync(Guid activityId)
    {
        return await _activityRepository.GetByIdAsync(activityId);
    }

    public async Task<byte[]> ExportActivitiesCsvAsync()
    {
        List<Activity> activities =
            await _activityRepository.GetAllAsync();

        return CsvExporter.Export<Activity>(
            activities,
            ("Title", (Activity a) => a.Title),
            ("Topic", (Activity a) => a.Topic),
            ("PlannedHours", (Activity a) =>
                a.PlannedHours.ToString(CultureInfo.InvariantCulture))
        );
    }

    public async Task<ActivityCsvImportResultDto> ImportActivitiesCsvAsync(
        Stream csvStream)
    {
        var result = new ActivityCsvImportResultDto();
        var activities = new List<Activity>();

        var options = new CsvReaderOptions
        {
            TrimWhitespace = true,
            TrackLineNumbers = true
        };

        using var csv = CsvReader.Create(
            csvStream,
            options,
            leaveOpen: true);

        try
        {
            await csv.ReadHeaderAsync();

            if (csv.Headers.Count == 0)
            {
                result.Errors.Add("The uploaded CSV file is empty.");
                return result;
            }

            // Find the required column indexes.
            bool hasTitle = csv.TryGetFieldIndex("Title", out var titleIndex);
            bool hasTopic = csv.TryGetFieldIndex("Topic", out var topicIndex);
            bool hasPlannedHours = csv.TryGetFieldIndex(
                "PlannedHours",
                out var plannedHoursIndex);

            var missingHeaders = new List<string>();

            if (!hasTitle)
                missingHeaders.Add("Title");

            if (!hasTopic)
                missingHeaders.Add("Topic");

            if (!hasPlannedHours)
                missingHeaders.Add("PlannedHours");

            if (missingHeaders.Count > 0)
            {
                result.Errors.Add(
                    $"Missing required columns: {string.Join(", ", missingHeaders)}");

                return result;
            }

            while (await csv.ReadAsync())
            {
                var rowNumber = csv.RowNumber;

                var title = csv.GetString(titleIndex).Trim();
                var topic = csv.GetString(topicIndex).Trim();
                var plannedHoursText = csv.GetString(plannedHoursIndex).Trim();

                // Skip completely empty rows.
                if (string.IsNullOrWhiteSpace(title) &&
                    string.IsNullOrWhiteSpace(topic) &&
                    string.IsNullOrWhiteSpace(plannedHoursText))
                {
                    continue;
                }

                var rowHasError = false;

                if (string.IsNullOrWhiteSpace(title))
                {
                    result.Errors.Add(
                        $"Row {rowNumber}: Activity title is required.");

                    rowHasError = true;
                }

                if (string.IsNullOrWhiteSpace(topic))
                {
                    result.Errors.Add(
                        $"Row {rowNumber}: Topic is required.");

                    rowHasError = true;
                }

                if (!decimal.TryParse(
                        plannedHoursText,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out var plannedHours) ||
                    plannedHours <= 0)
                {
                    result.Errors.Add(
                        $"Row {rowNumber}: Planned hours must be a valid number greater than zero.");

                    rowHasError = true;
                }

                if (rowHasError)
                    continue;

                activities.Add(new Activity
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Topic = topic,
                    PlannedHours = plannedHours
                });
            }
        }
        catch (CsvFormatException)
        {
            result.Errors.Add(
                "The CSV file contains invalid formatting. Please check the file and try again.");

            return result;
        }

        if (activities.Count == 0 && result.Errors.Count == 0)
        {
            result.Errors.Add(
                "The CSV file does not contain any activities.");
        }

        // Import nothing if any row contains an error.
        if (result.Errors.Count > 0)
            return result;

        await _activityRepository.AddRangeAsync(activities);

        await _activityRepository.SaveChangesAsync();

        result.ImportedCount = activities.Count;

        return result;
    }
}
