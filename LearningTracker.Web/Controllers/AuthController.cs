
using System.Security.Claims;
using LearningTracker.Models.DTOs;
using LearningTracker.Services.Interfaces;
using LearningTracker.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dto = new AuthDto
        {
            Email = model.Email,
            Password = model.Password
        };

        var account = await _authService.AuthenticateAsync(dto);

        if (account is null)
        {
            ModelState.AddModelError(
                string.Empty,
                "Invalid email or password, or temporary password expired.");

            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new(ClaimTypes.Name, account.NormalizedEmail),
            new(ClaimTypes.Role, account.Role.ToString()),
            new("SecurityStamp", account.SecurityStamp),
            new("MustChangePassword", account.MustChangePassword.ToString().ToLowerInvariant())
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            "LearningTrackerCookie",
            principal,
            new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            });

        if (account.MustChangePassword)
            return RedirectToAction("ChangePassword");

        if (account.Role.ToString() == "Coordinator")
            return RedirectToAction("Index", "Dashboard");

        return RedirectToAction("Index", "MyTraining");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpGet]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var accountIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(accountIdClaim, out var accountId))
            return Challenge("LearningTrackerCookie");

        var dto = new AuthDto
        {
            AccountId = accountId,
            CurrentPassword = model.CurrentPassword,
            NewPassword = model.NewPassword
        };

        var success = await _authService.ChangePasswordAsync(dto);

        if (!success)
        {
            ModelState.AddModelError(
                string.Empty,
                "Current password is incorrect or has expired.");

            return View(model);
        }

        await HttpContext.SignOutAsync("LearningTrackerCookie");

        TempData["SuccessMessage"] =
            "Password changed successfully. Please log in again.";

        return RedirectToAction("Login");
    }

    [HttpPost]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("LearningTrackerCookie");

        return RedirectToAction("Login");
    }
}
