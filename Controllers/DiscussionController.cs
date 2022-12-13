using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenDiscussion_AutentificareIdentity.Data;
using OpenDiscussion_AutentificareIdentity.Models;
using System;
using System.Linq;

namespace OpenDiscussion_AutentificareIdentity.Controllers
{
    [Authorize]
    public class DiscussionController : Controller
    {

        private readonly ApplicationDbContext db;

        private readonly UserManager<AppUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public DiscussionController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }

        /// Afisare Discutii + Categorii + Userul care a postat subiectul 
        /// HttpGet implicit
        /*[Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Index()
        {
            var discussions = db.Discussions.Include("Category").Include("User");

            ViewBag.Discussions = discussions;

            if(TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        
        [Authorize(Roles ="User,Editor,Admin")]
        public IActionResult Show(int id)
        {
            Discussion discussion = db.Discussions.Include("Category").Include("User").Include("Comments").Include("Comments.User").Where(d => d.DiscussionId == id).First();
            SetAccessRights();
            return View(discussion);
        }

        /// Adaugarea unui comentariu
        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show([FromForm] Comment comment)
        {
            comment.DateComm = DateTime.Now;
            comment.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Discussion/Show/" + comment.DiscussionId);
            }

            else
            {
                Discussion discussion = db.Discussions.Include("Category")
                                         .Include("User")
                                         .Include("Comments")
                                         .Include("Comments.User")
                                         .Where(discussion => discussion.DiscussionId == comment.DiscussionId)
                                         .First();

                //return Redirect("/Discussion/Show/" + comm.ArticleId);

                // Adaugam bookmark-urile utilizatorului pentru dropdown
                ViewBag.UserBookmarks = db.Bookmarks
                                          .Where(b => b.UserId == _userManager.GetUserId(User))
                                          .ToList();
               
                SetAccessRights();

                return View(discussion);
            }


        }

        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("Editor"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.UserCurent = _userManager.GetUserId(User);
        }*/
    }
}
