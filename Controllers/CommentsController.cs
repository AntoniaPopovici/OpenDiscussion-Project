using Microsoft.AspNetCore.Mvc;

namespace OpenDiscussion_AutentificareIdentity.Controllers
{
    public class CommentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
