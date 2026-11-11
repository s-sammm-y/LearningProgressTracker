using LearningTracker.Services.Interfaces;
using LearningTracker.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Web.Controllers;

[Authorize(Policy = "CoordinatorOnly")]
public class ActivitiesController : Controller
{
    private readonly IActivityService _activityService;

    public ActivitiesController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var activities = await _activityService.GetAllActivitiesAsync(
            cancellationToken);

        return View(activities);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateActivityViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _activityService.CreateActivityAsync(
                model.Title,
                model.Topic,
                model.PlannedHours,
                cancellationToken);

            TempData["SuccessMessage"] = "Activity created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(
        Guid id,
        CancellationToken cancellationToken)
    {
        var activity = await _activityService.GetActivityByIdAsync(
            id, cancellationToken);

        if (activity == null)
            return NotFound();

        var model = new EditActivityViewModel
        {
            Id = activity.Id,
            Title = activity.Title,
            Topic = activity.Topic,
            PlannedHours = activity.PlannedHours
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        EditActivityViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var updated = await _activityService.UpdateActivityAsync(
                model.Id,
                model.Title,
                model.Topic,
                model.PlannedHours,
                cancellationToken);

            if (!updated)
                return NotFound();

            TempData["SuccessMessage"] = "Activity updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }
}