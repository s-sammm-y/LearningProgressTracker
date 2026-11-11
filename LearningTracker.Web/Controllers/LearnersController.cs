using LearningTracker.Services.Interfaces;
using LearningTracker.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Web.Controllers;

[Authorize(Policy = "CoordinatorOnly")]
public class LearnersController : Controller
{
    private readonly ILearnerService _learnerService;
    private readonly IAssignmentService _assignmentService;

    public LearnersController(
        ILearnerService learnerService,
        IAssignmentService assignmentService)
    {
        _learnerService = learnerService;
        _assignmentService = assignmentService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateLearnerViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var result = await _learnerService.CreateLearnerAsync(
                model.DisplayName,
                model.Email,
                model.Cohort,
                cancellationToken);

            TempData["LearnerName"] = result.Learner.DisplayName;
            TempData["LearnerEmail"] = model.Email;
            TempData["TemporaryPassword"] = result.TemporaryPassword;

            return RedirectToAction(nameof(Credentials));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Credentials()
    {
        if (TempData["TemporaryPassword"] is null)
            return RedirectToAction(nameof(Create));

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var learners = await _learnerService.GetAllLearnersAsync(
            cancellationToken);

        return View(learners);
    }

    [HttpGet]
    public async Task<IActionResult> Details(
        Guid id,
        CancellationToken cancellationToken)
    {
        var learner = await _learnerService.GetLearnerByIdAsync(
            id,
            cancellationToken);

        if (learner == null)
            return NotFound();

        var assignments = await _assignmentService.GetAssignmentsForLearnerAsync(
            learner.AccountId,
            cancellationToken);

        var viewModel = new LearnerDetailsViewModel
        {
            Learner = learner,
            Assignments = assignments
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(
        Guid id,
        CancellationToken cancellationToken)
    {
        var temporaryPassword =
            await _learnerService.ResetTemporaryPasswordAsync(
                id,
                cancellationToken);

        if (temporaryPassword == null)
            return NotFound();

        TempData["ResetTemporaryPassword"] = temporaryPassword;

        TempData["SuccessMessage"] =
            "Temporary password reset successfully. Share the new credentials privately.";

        return RedirectToAction(nameof(Details), new { id });
    }
}