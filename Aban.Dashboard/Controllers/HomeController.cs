using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban.Dashboard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            if (User.IsInRole("Foreman") || User.IsInRole("Clerk"))
            {
                return RedirectToAction("LatestBlogArticles", "BlogArticle", new { area = "Blog", isvisible = true });
            }

            return View();
        }

    }
}