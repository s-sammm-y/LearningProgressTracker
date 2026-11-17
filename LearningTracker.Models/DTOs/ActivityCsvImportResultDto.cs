using System;

namespace LearningTracker.Models.DTOs;

public class ActivityCsvImportResultDto
{
    public int ImportedCount { get; set; }

    public List<string> Errors { get; set; } = new();

    public bool Success => Errors.Count == 0;
}
