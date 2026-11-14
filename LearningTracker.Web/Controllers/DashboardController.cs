using LearningTracker.Services.Interfaces;
using LearningTracker.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Web.Controllers;

[Authorize(Policy = "CoordinatorOnly")]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index(
    CancellationToken cancellationToken)
    {
        var viewModel = new DashboardViewModel
        {
            TotalLearners = await _dashboardService
                .GetTotalLearnersAsync(cancellationToken),

            TotalActivities = await _dashboardService
                .GetTotalActivitiesAsync(cancellationToken),

            TotalAssignments = await _dashboardService
                .GetTotalAssignmentsAsync(cancellationToken),

            CompletedAssignments = await _dashboardService
                .GetCompletedAssignmentsAsync(cancellationToken),

            InProgressAssignments = await _dashboardService
                .GetInProgressAssignmentsAsync(cancellationToken),

            Activities = await _dashboardService
                .GetActivitiesAsync(cancellationToken),

            CohortProgress = await _dashboardService
                .GetCohortProgressAsync(cancellationToken)
        };

        return View(viewModel);
    }
}