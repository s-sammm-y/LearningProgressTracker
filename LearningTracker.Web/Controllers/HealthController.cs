using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Web.Controllers;

[AllowAnonymous]
public class HealthController : Controller
{
    [HttpGet("/health")]
    public IActionResult Check()
    {
        return Content("Healthy");
    }
}
