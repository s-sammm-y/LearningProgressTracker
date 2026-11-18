using System;
using LearningTracker.Models.DTOs;
using LearningTracker.Models.Entities;

namespace LearningTracker.Services.Interfaces;

public interface IDashboardService
{
    Task<int> GetTotalLearnersAsync();

    Task<int> GetTotalActivitiesAsync();

    Task<int> GetTotalAssignmentsAsync();

    Task<int> GetCompletedAssignmentsAsync();

    Task<List<CohortProgress>> GetCohortProgressAsync();
    
    Task<int> GetInProgressAssignmentsAsync();

    Task<List<Activity>> GetActivitiesAsync();
}
