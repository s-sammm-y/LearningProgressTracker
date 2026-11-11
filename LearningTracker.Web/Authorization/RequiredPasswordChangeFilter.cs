
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearningTracker.Web.Authorization;

public class RequirePasswordChangeFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var mustChangePassword = context.HttpContext.User
            .FindFirst("MustChangePassword")?.Value;

        var controller = context.RouteData.Values["controller"]?.ToString();
        var action = context.RouteData.Values["action"]?.ToString();

        var isAllowedAction =
            controller == "Auth" &&
            (action == "ChangePassword" || action == "Logout");

        if (mustChangePassword == "true" && !isAllowedAction)
        {
            context.Result = new RedirectToActionResult(
                "ChangePassword",
                "Auth",
                null);

            return;
        }

        await next();
    }
}
