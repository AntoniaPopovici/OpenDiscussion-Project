using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDiscussion_AutentificareIdentity.Data;
using OpenDiscussion_AutentificareIdentity.Models;

namespace OpenDiscussionPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db;

        // USERS
        public IActionResult Index()
        {
            var users = from user in db.AppUsers
                        orderby user.UserName
                        select user;

            ViewBag.UsersList = users;
            return View();
        }

        // SHOW
        public IActionResult Show(string id)
        {
            AppUser user = db.AppUsers.Find(id);

            ViewBag.User = user;
            return View(user);
        }

        // NEW
        public IActionResult New()
        {
            return View();
        }
        public IActionResult New(AppUser s)
            
        {
            try
            {
                db.AppUsers.Add(s);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        // EDIT
        public IActionResult Edit(int id)
        {
            AppUser user = db.AppUsers.Find(id);
            ViewBag.AppUser = user;
            return View();
        }
        
        public ActionResult Edit(int id, AppUser requestUser)
        {
            AppUser user = db.AppUsers.Find(id);
            try
            {
                user.FirstName = requestUser.FirstName;
                user.LastName = requestUser.LastName;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", user.UserID);
            }
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            AppUser user = db.AppUsers.Find(id);
            db.AppUsers.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}