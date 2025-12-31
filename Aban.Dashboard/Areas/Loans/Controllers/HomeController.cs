using Aban.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban.Dashboard.Areas.Loans.Controllers
{
    [Area("Loans")]
    [Authorize]
    public class HomeController : GenericController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
