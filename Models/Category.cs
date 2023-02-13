using System.ComponentModel.DataAnnotations;

namespace OpenDiscussion_AutentificareIdentity.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please fill in the required field.")]
        public string CategoryName { get; set; }
        public virtual ICollection<Discussion>? Discussions { get; set; }
    }
}
