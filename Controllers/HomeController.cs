//verificare
using Microsoft.AspNetCore.Mvc;
using OpenDiscussion_AutentificareIdentity.Models;
using System.Diagnostics;

namespace OpenDiscussion_AutentificareIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Categories");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "mesaj";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "mesaj";

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}