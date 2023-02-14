using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenDiscussion_AutentificareIdentity.Models
{
    public class Discussion
    {
        [Key]
        public int DiscussionId { get; set; }
        [Required(ErrorMessage = "Please fill in the required field")]
        [MinLength(5,ErrorMessage = "Title should have between 5 and 25 characters")]
        [StringLength(25, ErrorMessage = "Title should have between 5 and 25 characters")]
        public string DiscussionName { get; set; }

        [Required(ErrorMessage = "Please fill in the required field")]
        public string Text { get; set; }
        public DateTime DateDiscussion { get; set; }

        [Required(ErrorMessage = "Please choose a cateegory")]
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem>? selectCategory { get; set; }
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }


    }
}
