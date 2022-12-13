using Microsoft.AspNetCore.Mvc;

namespace OpenDiscussion_AutentificareIdentity.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
