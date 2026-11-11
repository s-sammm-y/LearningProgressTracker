using LearningTracker.Services.Interfaces;
using LearningTracker.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Web.Controllers;

[Authorize(Policy = "CoordinatorOnly")]
public class LearnersController : Controller
{
    private readonly ILearnerService _learnerService;

    public LearnersController(ILearnerService learnerService)
    {
        _learnerService = learnerService;
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
}