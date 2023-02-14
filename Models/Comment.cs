using System.ComponentModel.DataAnnotations;

namespace OpenDiscussion_AutentificareIdentity.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required(ErrorMessage = "Please fill in the required field")]
        public string Content { get; set; }
        public DateTime DateComm { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
        public int? DiscussionId { get; set; }
        public virtual Discussion? Discussion { get; set; }


    }
}
