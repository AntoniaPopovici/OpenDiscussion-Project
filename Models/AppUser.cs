using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


/// Autentificare Pas 1
namespace OpenDiscussion_AutentificareIdentity.Models
{
    public class AppUser : IdentityUser
    {

        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Discussion>? Discussions { get; set;}

        public string? FirstName { get; set; }
        public string? LastName { get; set;}


        [NotMapped]
        public IEnumerable <SelectListItem>? AllRoles { get; set; }
    }
}
