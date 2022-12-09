using System.ComponentModel.DataAnnotations;

namespace OpenDiscussion_AutentificareIdentity.Models
{
    public class Discussion
    {
        [Key]
        public int DiscussionId { get; set; }
        [Required(ErrorMessage = "Acest camp trebuie completat!")]
        public string Text { get; set; }
        public DateTime DateDiscussion { get; set; }

        [Required(ErrorMessage = "Alege categoria!")]
        public int CategoryId { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }


    }
}
