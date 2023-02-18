using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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


		public async Task<ActionResult> Edit(string id)
		{
			AppUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var roleNames = await _userManager.GetRolesAsync(user); // lista de nume de roluri

            var currentUserRolse = _roleManager.Roles
                                               .Where(role => roleNames.Contains(role.Name))
                                               .Select(role => role.Id)
                                               .First();    /// se selecteaza un singur rol
            ViewBag.UserRole = currentUserRolse;
            return View(user);
		}


        [HttpPost]
        public async Task<ActionResult> Edit(string id, AppUser newUser, [FromForm] string newRole)
        {
            AppUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();

            if(ModelState.IsValid)
            {
                user.UserName = newUser.UserName;
                user.Email = newUser.Email;
                user.FirstName = newUser.FirstName;
                user.LastName = newUser.LastName;
                user.PhoneNumber = newUser.PhoneNumber;

                var roles = db.Roles.ToList();
                foreach(var role in roles)
                {
                    //Scoatem userul din rolurile anterioare
                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                }

                ///Adaugam noul rol selectat
                var roleName = await _roleManager.FindByIdAsync(newRole);
                await _userManager.AddToRoleAsync(user, roleName.ToString());
                db.SaveChanges();


            }
            return RedirectToAction("Index");


        }


		[NonAction]
		public IEnumerable<SelectListItem> GetAllRoles()
		{
			var selectList = new List<SelectListItem>();

			var roles = from role in db.Roles
						select role;

			foreach (var role in roles)
			{
				selectList.Add(new SelectListItem
				{
					Value = role.Id.ToString(),
					Text = role.Name.ToString()
				});
			}
			return selectList;
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

       // [Authorize(Roles = "Admin")]
        //public IActionResult Edit(string id)
        //{
          /*  var user = db.Users.FirstOrDefault(x => x.Id == id);
            var userRole = db.UserRoles
                                    .Where(ur => ur.UserId == id)
                                    .ToList();
            ViewBag.roles = db.Roles.ToList();
            ViewBag.userRole = userRole[0];
            return View(user);
        }


        [Authorize(Roles = "Admin")]
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
                return RedirectToAction("Edit", user.Id);
            }
        }
          */
        // DELETE

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var user = db.AppUsers
                         .Where(u => u.Id == id)
                         .First();
            db.AppUsers.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}