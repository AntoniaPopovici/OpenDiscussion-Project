using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDiscussion_AutentificareIdentity.Data;
using OpenDiscussion_AutentificareIdentity.Models;



namespace OpenDiscussion_AutentificareIdentity.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly ApplicationDbContext db;

        public CategoriesController(ApplicationDbContext context)
        {
            db = context;
        }
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

            ViewBag.Categories = categories;

            SetAccessRights();
            return View();
        }

        // SHOW
        public IActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            SetAccessRights();
            return View(category);
        }

        [Authorize(Roles ="Admin")]
        public ActionResult New()
        {   
            Category category = new Category();
            return View(category);
        }

        // NEW
        [Authorize(Roles = "Admin")]
        public ActionResult New(Category category)
        {
            if(ModelState.IsValid)
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

        public ActionResult Edit(int id, Category requestCategory)
        {
            Category category = db.Categories.Find(id);

            if(ModelState.IsValid)
            {
                category.CategoryName = requestCategory.CategoryName;
                db.SaveChanges();
                TempData["message"] = "Categoria a fost modificata";
                return RedirectToAction("Index");
            }
            else
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


        }
        #endregion

    }

}