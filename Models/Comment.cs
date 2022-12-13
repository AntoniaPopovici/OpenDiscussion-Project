using System.ComponentModel.DataAnnotations;

namespace OpenDiscussion_AutentificareIdentity.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required(ErrorMessage = "Continutul este obligatoriu")]
        public string Content { get; set; }
        public DateTime DateComm { get; set; }

        public int? DiscussionId { get; set; }
        public virtual Discussion? Discussion { get; set; }


    }
}
