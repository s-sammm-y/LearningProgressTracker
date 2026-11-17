
using System.Text;
using LearningTracker.Models.DTOs;
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
            var dto = new ActivityDto
            {
                Title = model.Title,
                Topic = model.Topic,
                PlannedHours = model.PlannedHours
            };

            await _activityService.CreateActivityAsync(
                dto,
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
            var dto = new ActivityDto
            {
                ActivityId = model.Id,
                Title = model.Title,
                Topic = model.Topic,
                PlannedHours = model.PlannedHours
            };

            var updated = await _activityService.UpdateActivityAsync(
                dto,
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


    [HttpGet]
    public IActionResult DownloadTemplate()
    {
        // Define the CSV column headers.
        var csvContent = "Title,Topic,PlannedHours\r\n";

        // Generate CSV bytes with UTF-8 BOM for Excel compatibility.
        var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
        var bytes = encoding.GetBytes(csvContent);

        // Return the CSV file for download.
        return File(
            bytes,
            "text/csv",
            "activity_import_template.csv");
    }

    [HttpGet]
    public async Task<IActionResult> ExportCsv(
        CancellationToken cancellationToken)
    {
        var csvBytes = await _activityService.ExportActivitiesCsvAsync(
            cancellationToken);

        return File(
            csvBytes,
            "text/csv",
            $"activities_{DateTime.UtcNow:yyyyMMdd}.csv");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> ImportCsv(
        IFormFile? file,
        CancellationToken cancellationToken)
    {
        // Check whether a file was uploaded.
        if (file == null || file.Length == 0)
        {
            TempData["ImportErrorMessage"] =
                "Please select a CSV file to upload.";

            return RedirectToAction(nameof(Create));
        }

        // Validate the file extension.
        if (!string.Equals(
                Path.GetExtension(file.FileName),
                ".csv",
                StringComparison.OrdinalIgnoreCase))
        {
            TempData["ImportErrorMessage"] =
                "Only CSV files are allowed.";

            return RedirectToAction(nameof(Create));
        }

        await using var stream = file.OpenReadStream();

        var result = await _activityService.ImportActivitiesCsvAsync(
            stream,
            cancellationToken);

        // If validation errors exist, don't redirect to Index.
        if (result.Errors.Count > 0)
        {
            TempData["ImportErrorMessage"] =
                string.Join(Environment.NewLine, result.Errors);

            return RedirectToAction(nameof(Create));
        }

        TempData["SuccessMessage"] =
            $"{result.ImportedCount} activities imported successfully.";

        return RedirectToAction(nameof(Index));
    }
}
