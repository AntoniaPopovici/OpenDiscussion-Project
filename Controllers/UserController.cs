using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenDiscussion_AutentificareIdentity.Data;
using OpenDiscussion_AutentificareIdentity.Models;

namespace OpenDiscussionPlatform.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private ApplicationDbContext db;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }



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
        public async Task<ActionResult> Show(string id)
        {
            AppUser user = db.AppUsers.Find(id);
            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.Roles = roles;
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