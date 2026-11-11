using LearningTracker.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            var model = new ErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier
            };

            return View("~/Views/Shared/Error.cshtml", model);
        }
    }
}