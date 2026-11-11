using LearningTracker.Services.Interfaces;
using LearningTracker.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearningTracker.Web.Controllers;

[Authorize(Policy = "CoordinatorOnly")]
public class AssignmentsController : Controller
{
    private readonly IAssignmentService _assignmentService;
    private readonly ILearnerService _learnerService;
    private readonly IActivityService _activityService;

    public AssignmentsController(
        IAssignmentService assignmentService,
        ILearnerService learnerService,
        IActivityService activityService)
    {
        _assignmentService = assignmentService;
        _learnerService = learnerService;
        _activityService = activityService;
    }

    [HttpGet]
    public async Task<IActionResult> Create(
        CancellationToken cancellationToken)
    {
        var model = new AssignActivityViewModel();

        await PopulateOptionsAsync(model, cancellationToken);

        return View(model);
    }

    private async Task PopulateOptionsAsync(
        AssignActivityViewModel model,
        CancellationToken cancellationToken)
    {
        var learners = await _learnerService.GetAllLearnersAsync(
            cancellationToken);

        var activities = await _activityService.GetAllActivitiesAsync(
            cancellationToken);

        model.Learners = learners.Select(learner =>
            new SelectListItem
            {
                Value = learner.Id.ToString(),
                Text = $"{learner.DisplayName} ({learner.Account.NormalizedEmail})"
            }).ToList();

        model.Activities = activities.Select(activity =>
            new SelectListItem
            {
                Value = activity.Id.ToString(),
                Text = $"{activity.Title} - {activity.PlannedHours} hours"
            }).ToList();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        AssignActivityViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateOptionsAsync(model, cancellationToken);
            return View(model);
        }

        try
        {
            await _assignmentService.AssignActivityAsync(
                model.LearnerId,
                model.ActivityId,
                cancellationToken);

            TempData["SuccessMessage"] =
                "Activity assigned successfully.";

            return RedirectToAction(nameof(Create));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            await PopulateOptionsAsync(model, cancellationToken);
            return View(model);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            await PopulateOptionsAsync(model, cancellationToken);
            return View(model);
        }
    }
}