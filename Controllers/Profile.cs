using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenDiscussion_AutentificareIdentity.Controllers
{
    public class Profile : Controller
    {
        [Authorize]
      
        public IActionResult Index()
        {
            return View();
        }
    }
}
