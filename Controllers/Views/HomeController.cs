using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.Views;

namespace webapi.Controllers.Views
{
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Pages/Home/Index.cshtml");
        }

        [Authorize]
        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View("~/Views/Pages/Home/Privacy.cshtml");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin")]
        public IActionResult Admin()
        {
            return View("~/Views/Pages/Home/Admin.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}