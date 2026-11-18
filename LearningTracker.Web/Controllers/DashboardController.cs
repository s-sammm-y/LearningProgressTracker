
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

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel
        {
            TotalLearners = await _dashboardService
                .GetTotalLearnersAsync(),

            TotalActivities = await _dashboardService
                .GetTotalActivitiesAsync(),

            TotalAssignments = await _dashboardService
                .GetTotalAssignmentsAsync(),

            CompletedAssignments = await _dashboardService
                .GetCompletedAssignmentsAsync(),

            InProgressAssignments = await _dashboardService
                .GetInProgressAssignmentsAsync(),

            Activities = await _dashboardService
                .GetActivitiesAsync(),

            CohortProgress = await _dashboardService
                .GetCohortProgressAsync()
        };

        return View(viewModel);
    }
}
