
using LearningTracker.Models.DTOs;
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
    public async Task<IActionResult> Create()
    {
        var model = new AssignActivityViewModel();

        await PopulateOptionsAsync(model);

        return View(model);
    }

    private async Task PopulateOptionsAsync(
        AssignActivityViewModel model)
    {
        var learners = await _learnerService.GetAllLearnersAsync();

        var activities = await _activityService.GetAllActivitiesAsync();

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
        AssignActivityViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateOptionsAsync(model);
            return View(model);
        }

        try
        {
            var dto = new AssignmentDto
            {
                LearnerId = model.LearnerId,
                ActivityId = model.ActivityId
            };

            await _assignmentService.AssignActivityAsync(dto);

            TempData["SuccessMessage"] =
                "Activity assigned successfully.";

            return RedirectToAction(nameof(Create));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            await PopulateOptionsAsync(model);
            return View(model);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            await PopulateOptionsAsync(model);
            return View(model);
        }
    }
}
