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
        var model = new DashboardViewModel
        {
            TotalLearners =
                await _dashboardService.GetTotalLearnersAsync(cancellationToken),

            TotalActivities =
                await _dashboardService.GetTotalActivitiesAsync(cancellationToken),

            TotalAssignments =
                await _dashboardService.GetTotalAssignmentsAsync(cancellationToken),

            CompletedAssignments =
                await _dashboardService.GetCompletedAssignmentsAsync(cancellationToken),

            CohortProgress = await _dashboardService.GetCohortProgressAsync()
        };

        return View(model);
    }
}