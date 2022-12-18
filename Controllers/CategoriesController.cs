using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDiscussion_AutentificareIdentity.Data;
using OpenDiscussion_AutentificareIdentity.Models;



namespace OpenDiscussion_AutentificareIdentity.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db;

        // CATEGORIES
        public ActionResult Index()
        {
            SetAccessRights();

            var categories = db.Categories;
            ViewBag.Categories = categories;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
            }

            IDictionary<Category, int> nrSubjects = new Dictionary<Category, int>();
            foreach (var i in categories)
            {
                List<int> subjectIds = db.Discussions.Where(s => s.CategoryId == i.CategoryId).Select(a => a.DiscussionId).ToList();
                nrSubjects.Add(i, subjectIds.Count());
            }
            ViewBag.nrSubjects = nrSubjects;

            return View();
        }

        // SHOW
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            SetAccessRights();
            return View(category);
        }

        // NEW
        [Authorize(Roles = "Admin")]
        public ActionResult New(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    TempData["message"] = "categorie noua";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(category);
                }
            }
            catch (Exception e)
            {
                return View(category);
            }
        }


        // EDIT
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = db.Categories.Find(id);
                    if (TryUpdateModel(category))
                    {
                        category.CategoryName = requestCategory.CategoryName;
                        category.CategoryDescription = requestCategory.CategoryDescription;
                        db.SaveChanges();
                        TempData["message"] = "editata";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(requestCategory);
                    }
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

        private bool TryUpdateModel(Category? category)
        {
            throw new NotImplementedException();
        }

        // DELETE
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["message"] = "Categoria a fost stearsa";
            return RedirectToAction("Index");
        }

        private void SetAccessRights()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("User"))
            {
                ViewBag.afisareButoane = true;
            }
            ViewBag.esteUser = User.IsInRole("User");
            ViewBag.esteAdmin = User.IsInRole("Admin");
        }
    }

}