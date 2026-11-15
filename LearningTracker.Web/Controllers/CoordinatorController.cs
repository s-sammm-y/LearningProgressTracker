
using System.Security.Cryptography;
using System.Text;
using LearningTracker.Services.Interfaces;
using LearningTracker.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Web.Controllers;

[AllowAnonymous]
public class CoordinatorController : Controller
{
    private readonly ICoordinatorService _coordinatorService;
    private readonly IConfiguration _configuration;

    public CoordinatorController(
        ICoordinatorService coordinatorService,
        IConfiguration configuration)
    {
        _coordinatorService = coordinatorService;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        CoordinatorRegistrationViewModel model,
        CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        if (!ModelState.IsValid)
            return View(model);

        // Retrieve the registration secret from configuration.
        var configuredSecret =
            _configuration["COORDINATOR_REGISTRATION_SECRET"];

        if (string.IsNullOrWhiteSpace(configuredSecret))
        {
            ModelState.AddModelError(
                string.Empty,
                "Coordinator registration is currently unavailable.");

            return View(model);
        }

        // Compare secrets without ordinary string comparison.
        var suppliedBytes = Encoding.UTF8.GetBytes(
            model.RegistrationSecret);

        var configuredBytes = Encoding.UTF8.GetBytes(
            configuredSecret);

        var isValidSecret = CryptographicOperations.FixedTimeEquals(
            suppliedBytes,
            configuredBytes);

        if (!isValidSecret)
        {
            ModelState.AddModelError(
                nameof(model.RegistrationSecret),
                "Invalid registration secret.");

            return View(model);
        }

        try
        {
            await _coordinatorService.CreateCoordinatorAsync(
                model.Email,
                model.Password,
                cancellationToken);

            TempData["SuccessMessage"] =
                "Coordinator account created successfully. You can now sign in.";

            return RedirectToAction("Login", "Auth");
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
}
