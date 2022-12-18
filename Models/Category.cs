using System.ComponentModel.DataAnnotations;

namespace OpenDiscussion_AutentificareIdentity.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Acest camp este obligatoriu.")]
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }
        public virtual ICollection<Discussion>? Discussions { get; set; }
    }
}
