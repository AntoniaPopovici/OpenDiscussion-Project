using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDiscussion_AutentificareIdentity.Data;
using OpenDiscussion_AutentificareIdentity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;


namespace OpenDiscussion_AutentificareIdentity.Controllers

{

    public class CategoriesController : Controller
    {

        private readonly ApplicationDbContext db;
        private int _perPage = 4;

        public CategoriesController(ApplicationDbContext context)
        {
            db = context;
        }
        [AllowAnonymous]
        //[Authorize(Roles = "User,Moderator,Admin")]
       public ActionResult Index(string sortOrder)
        {
            var categories = from category in db.Categories
                             orderby category.CategoryName
                             select category;

            ViewBag.Categories = categories;
            ViewBag.Title = "Categorii";

            if (TempData.ContainsKey("message"))
            {
                ViewBag.AlertMsg = TempData["message"].ToString();
            }

            if (!String.IsNullOrEmpty(HttpContext.Request.Query["search"]) && !String.IsNullOrWhiteSpace(HttpContext.Request.Query["search"]))
            {
                ICollection<Discussion>? discussions;

                if (sortOrder == "resp")
                {
                    discussions = db.Discussions.Include("Category")
                                      .Include("User")
                                      .OrderByDescending(
                                                        top =>
                                                        db.Comments
                                                        .Where(resp => resp.CommentId == top.DiscussionId)
                                                        .Count()
                                                        )
                                      .ThenByDescending(top => top.DateDiscussion)
                                      .ToList();
                }
                else
                {
                    discussions = db.Discussions.Include("Category")
                                      .Include("User")
                                      .OrderByDescending(top => top.DateDiscussion)
                                      .ToList();
                }

                // CAUTARE
                string search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();

                List<int> discussionIds = db.Discussions.Where(
                                                    top => top.DiscussionName.Contains(search)).Select(t => t.DiscussionId).ToList();

                List<int> discussionIdsOfCommentsWithSearchString = db.Comments.Where(rsp => rsp.Content.Contains(search))
                                                                            .Select(r => r.DiscussionId.GetValueOrDefault())
                                                                            .ToList();

                List<int> mergedIds = discussionIds.Union(discussionIdsOfCommentsWithSearchString).ToList();

                if (sortOrder == "resp")
                {
                    discussions = db.Discussions.Include("Category")
                                      .Include("User")
                                      .Where(discussion => mergedIds.Contains(discussion.DiscussionId))
                                      .OrderByDescending(
                                                    top =>
                                                    db.Comments
                                                    .Where(resp => resp.DiscussionId == top.CategoryId)
                                                    .Count()
                                                    )
                                      .ThenByDescending(top => top.DateDiscussion)
                                      .ToList();
                }
                else
                {
                    discussions = db.Discussions.Include("Category")
                                      .Include("User")
                                      .Where(
                                             discussion => mergedIds.Contains(discussion.DiscussionId)
                                            )
                                      .OrderByDescending(t => t.DateDiscussion)
                                      .ToList();
                }

                ViewBag.SearchString = search;

                //AFISARE PAGINATA
                int _perPage = 3;
                int totalItems = discussions.Count();
                var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);
                var offset = 0;

                if (!currentPage.Equals(0))
                {
                    offset = (currentPage - 1) * _perPage;
                }

                var paginatedDiscussions = discussions.Skip(offset).Take(_perPage);
                ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);
                ViewBag.Discussions = paginatedDiscussions;
                ViewBag.PaginationBaseUrl = "/Categories/Index/?search=" + search + "&sortOrder=" + sortOrder + "&page";
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Show(int id, string sortOrder)
        {
            Category category = db.Categories.Find(id);
            ICollection<Discussion>? discussions;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.AlertMsg = TempData["message"].ToString();
            }

            if (sortOrder == "resp")
            {
                discussions = db.Discussions.Include("Category")
                                  .Include("User")
                                  .Where(top => top.CategoryId == id)
                                  .OrderByDescending(
                                                    top =>
                                                    db.Comments
                                                    .Where(resp => resp.CommentId == top.DiscussionId)
                                                    .Count()
                                                    )
                                  .ThenByDescending(top => top.DateDiscussion)
                                  .ToList();
            }
            else
            {
                discussions = db.Discussions.Include("Category")
                                  .Include("User")
                                  .Where(top => top.CategoryId == id)
                                  .OrderByDescending(top => top.DateDiscussion)
                                  .ToList();
            }

            string search = null;

            // MOTOR DE CAUTARE

            if (!String.IsNullOrEmpty(HttpContext.Request.Query["SearchString"]) && !String.IsNullOrWhiteSpace(HttpContext.Request.Query["search"]))
            {
                search = Convert.ToString(HttpContext.Request.Query["SearchString"]).Trim();

                List<int> discussionIds = db.Discussions.Where(top => top.DiscussionName.Contains(search))
                                              .Select(t => t.DiscussionId).ToList();


                List<int> discussionIdsOfCommentsWithSearchString = db.Comments.Where(rsp => rsp.Content.Contains(search))
                                                                            .Select(r => r.DiscussionId.GetValueOrDefault())
                                                                            .ToList();

                List<int> mergedIds = discussionIds.Union(discussionIdsOfCommentsWithSearchString).ToList();

                if (sortOrder == "SearchString")
                {
                    discussions = db.Discussions.Include("Category")
                                      .Include("User")
                                      .Where(discussion => mergedIds.Contains(discussion.DiscussionId) && discussion.CategoryId == id)
                                      .OrderByDescending(top => db.Comments.Where(resp => resp.DiscussionId == top.CategoryId).Count())
                                      .ThenByDescending(top => top.DateDiscussion)
                                      .ToList();
                }
                else
                {
                    discussions = db.Discussions.Include("Category")
                                      .Include("User")
                                      .Where(discussion => mergedIds.Contains(discussion.DiscussionId) && discussion.CategoryId == id)
                                      .OrderByDescending(t => t.DateDiscussion)
                                      .ToList();
                }

            }

            ViewBag.SearchString = search;

            //AFISARE PAGINATA

            int _perPage = 3;
            int totalItems = discussions.Count();
            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);
            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            var paginatedDiscussions = discussions.Skip(offset).Take(_perPage);
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);
            ViewBag.Discussions = paginatedDiscussions;
            ViewBag.CategoryName = category.CategoryName;

            if (!String.IsNullOrEmpty(HttpContext.Request.Query["search"]) && !String.IsNullOrWhiteSpace(HttpContext.Request.Query["search"]))
            {
                ViewBag.PaginationBaseUrl = "/Categories/Show/" + id + "?search="
                + search + "&sortOrder=" + sortOrder + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/Categories/Show/" + id + "?sortOrder=" + sortOrder + "&page";
            }

            SetAccessRights();

            return View();
        }

        /*
        // CATEGORIES
        
        public ActionResult Index()
        {

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
            }

            var categories = from category in db.Categories
                             orderby category.CategoryName
                             select category;
            var discussions = from discussion in db.Discussions
                              select discussion;

            ViewBag.Categories = categories;
            ViewBag.Discussions = discussions;

            

            SetAccessRights();
            return View();
        }

        // SHOW
        public ActionResult Show(int id, string sort)
        {
            Category category = db.Categories.Find(id);
            Discussion[] discussions;

            switch (sort)
            {
                case "dateAsc":
                    discussions = category.Discussions.OrderBy(s => s.DateDiscussion).ToArray();
                    break;
                case "dateDesc":
                    discussions = category.Discussions.OrderByDescending(s => s.DateDiscussion).ToArray();
                    break;
                case "titleAsc":
                    discussions = category.Discussions.OrderBy(s => s.DiscussionName).ToArray();
                    break;
                case "titleDesc":
                    discussions = category.Discussions.OrderByDescending(s => s.DiscussionName).ToArray();
                    break;
                default:
                    discussions = category.Discussions.OrderByDescending(s => s.DateDiscussion).ToArray();
                    break;
            }


            var totalItems = discussions.Count();
            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }

            var paginatedDisc = discussions.Skip(offset).Take(this._perPage);

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Discussions = paginatedDisc;
            ViewBag.sort = sort;

            SetAccessRights();
            return View(category);
        }
        */
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            Category category = new Category();
            return View(category);
        }

        // NEW
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata";
                return RedirectToAction("Index");
            }

            else
            {
                return View(category);
            }
        }

        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }


        [HttpPost]
        public ActionResult Edit(int id, Category requestCategory)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var category = db.Categories.Find(id);
                    category.CategoryName = requestCategory.CategoryName;
                    db.SaveChanges();
                    TempData["message"] = "Categoria a fost editata!";
                    return RedirectToAction("Index");

                }
                else
                {
                    return View(requestCategory);
                }
            }
            catch (Exception)
            {
                return View(requestCategory);
            }
        }

        // DELETE
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["message"] = "Categoria a fost stearsa";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region Helpers

        private void SetAccessRights()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("User"))
            {
                ViewBag.afisareButoane = true;
            }
            ViewBag.esteUser = User.IsInRole("User");
            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.esteModerator = User.IsInRole("Editor");
            ViewBag.currentUser = User.Identity.GetUserId();


        }
        #endregion

    }

}