using System.Security.Claims;
using LearningTracker.Services.Interfaces;
using LearningTracker.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Web.Controllers;

[Authorize(Policy = "CandidateOnly")]
public class MyTrainingController : Controller
{
    private readonly IAssignmentService _assignmentService;

    public MyTrainingController(
        IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var accountIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(accountIdValue, out var accountId))
            return Forbid();

        var assignments =
            await _assignmentService.GetAssignmentsForLearnerAsync(
                accountId,
                cancellationToken);

        return View(assignments);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(
        UpdateAssignmentStatusViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Index));

        var accountIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(accountIdValue, out var accountId))
            return Forbid();

        var updated = await _assignmentService.UpdateStatusAsync(
            model.AssignmentId,
            accountId,
            model.Status,
            cancellationToken);

        if (!updated)
            return NotFound();

        TempData["SuccessMessage"] = "Training status updated successfully.";

        return RedirectToAction(nameof(Index));
    }
}