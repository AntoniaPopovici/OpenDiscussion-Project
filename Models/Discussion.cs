using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenDiscussion_AutentificareIdentity.Models
{
    public class Discussion
    {
        [Key]
        public int DiscussionId { get; set; }
        [Required(ErrorMessage = "Acest camp este obligatoriu")]
        [MinLength(5,ErrorMessage = "Lungimea titlului")]
        public string DiscussionName { get; set; }
        [Required(ErrorMessage = "Acest camp trebuie completat!")]
        public string Text { get; set; }
        public DateTime DateDiscussion { get; set; }

        [Required(ErrorMessage = "Alege categoria!")]
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem>? selectCategory { get; set; }
        public int? UserId { get; set; }
        public virtual AppUser? User { get; set; }


    }
}
