using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OpenDiscussion_AutentificareIdentity.Data;
using OpenDiscussion_AutentificareIdentity.Models;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Index()
        {
            var discussions = db.Discussions.Include("Category").Include("User");

            ViewBag.Discussions = discussions;

            var search = "";

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere 

                /// cautare discutie Titlu + Continut
                List<int> discussionIds = db.Discussions.Where
                                           (discussion => discussion.DiscussionName.Contains(search)
                                            || discussion.Text.Contains(search)
                                            ).Select(d => d.DiscussionId).ToList();

                // cautare in comentarii (content)
                List<int> discussionIdsofComments = db.Comments.Where
                                            (comment => comment.Content.Contains(search)
                                            ).Select(comment => (int)comment.DiscussionId).ToList();

                List<int> mergedIds = discussionIds.Union(discussionIdsofComments).ToList();

                discussions = db.Discussions.Where
                              (discussion => mergedIds.Contains(discussion.DiscussionId))
                              .Include("Category")
                              .Include("User")
                              .OrderBy(discussion => discussion.DateDiscussion);
            }

            ViewBag.SearchString = search;

            if(TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
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
               /* ViewBag.UserBookmarks = db.Bookmarks
                                          .Where(b => b.UserId == _userManager.GetUserId(User))
                                          .ToList();*/
               
                SetAccessRights();

                return View(discussion);
            }


        }


        [Authorize(Roles = "Editor,Admin")]
        public IActionResult New()
        {
            Discussion discussion = new Discussion();

            // Se preia lista de categorii din metoda GetAllCategories()
            discussion.selectCategory = GetAllCategories();

            return View(discussion);
        }

        [Authorize(Roles = "Editor,Admin")]
        [HttpPost]

        public IActionResult New(Discussion discussion)
        {
            var sanitizer = new HtmlSanitizer();

            discussion.DateDiscussion = DateTime.Now;
            discussion.UserId = _userManager.GetUserId(User);


            if (ModelState.IsValid)
            {
                discussion.Text = sanitizer.Sanitize(discussion.Text);

                db.Discussions.Add(discussion);
                db.SaveChanges();
                TempData["message"] = "Ai adaugat un subiect nou de conversatie";
                return RedirectToAction("Index");
            }
            else
            {
                discussion.selectCategory = GetAllCategories();
                return View(discussion);
            }
        }


        [Authorize(Roles = "Editor,Admin")]
        public IActionResult Edit(int id)
        {

            Discussion discussion = db.Discussions.Include("Category")
                                        .Where(dis => dis.DiscussionId == id)
                                        .First();

            discussion.selectCategory = GetAllCategories();

            if (discussion.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(discussion);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra acestei postari";
                return RedirectToAction("Index");
            }

        }

        // Se adauga articolul modificat in baza de date
        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult Edit(int id, Discussion requestDiscussion)
        {
            var sanitizer = new HtmlSanitizer();

            Discussion discussion = db.Discussions.Find(id);


            if (ModelState.IsValid)
            {
                if (discussion.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    discussion.DiscussionName = requestDiscussion.DiscussionName;

                    requestDiscussion.Text = sanitizer.Sanitize(requestDiscussion.Text);

                    discussion.Text = requestDiscussion.Text;

                    discussion.CategoryId = requestDiscussion.CategoryId;
                    TempData["message"] = "Postarea a fost modificata";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra acestei postari";
                    return RedirectToAction("Index");
                }
            }
            else
            { 
                requestDiscussion.selectCategory = GetAllCategories();
                return View(requestDiscussion);
            }
        }


        // Se sterge o postare din baza de date 
        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Delete(int id)
        {
            Discussion discussion = db.Discussions.Include("Comments")
                                         .Where(dis => dis.DiscussionId == id)
                                         .First();

            if (discussion.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Discussions.Remove(discussion);
                db.SaveChanges();
                TempData["message"] = "Postarea a fost stearsa";
                return RedirectToAction("Index");
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti aceasta postare.";
                return RedirectToAction("Index");
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista de tipul SelectListItem fara elemente
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
          
            foreach (var category in categories)
            {
                var listItem = new SelectListItem();
                listItem.Value = category.CategoryId.ToString();
                listItem.Text = category.CategoryName.ToString();

                selectList.Add(listItem);
             }

            // returnam lista de categorii
            return selectList;
        }

        public IActionResult IndexNou()
        {
            return View();
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
        }
    }
}
