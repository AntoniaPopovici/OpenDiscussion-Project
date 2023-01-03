/* 
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using OpenDiscussion_AutentificareIdentity.Models;
using Owin;
[assembly: OwinStartupAttribute(typeof(OpenDiscussion_AutentificareIdentity.Startup))]
namespace OpenDiscussionPlatform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // adaugare admin si alte roluri]
            CreateAdminUserAndApplicationRoles();
        }

        private void CreateAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var UserManager = new UserManager<AppUser>(new UserStore<AppUser>(context));

            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("Admin"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                // se adauga utilizatorul administrator
                var user = new AppUser();
                user.UserName = "admin@gmail.com";
                user.Email = "admin@gmail.com";
                var adminCreated = UserManager.Create(user, "!1Admin");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("Moderator"))
            {
                var role = new IdentityRole();
                role.Name = "Moderator";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}

*/