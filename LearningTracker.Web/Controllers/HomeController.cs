using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}