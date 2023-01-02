using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

/// Autentificare Pas 1
namespace OpenDiscussion_AutentificareIdentity.Models
{
    public class AppUser : IdentityUser
    {

        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Discussion>? Discussions { get; set;}
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set;}


        [NotMapped]
        public IEnumerable <SelectListItem>? AllRoles { get; set; }

        public class ApplicationDbContext1 : IdentityDbContext<AppUser>
        {           

            public DbSet<Category> Categories { get; set; }
            public DbSet<Discussion> Discussion { get; set; }
            public DbSet<Comment> Comment { get; set; }

            public static ApplicationDbContext1 Create()
            {
                return new ApplicationDbContext1();
            }
        }
    }
}
