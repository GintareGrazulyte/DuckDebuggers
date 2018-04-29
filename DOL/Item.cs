using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DOL.Objects
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Price { get; set; }
        [NotMapped]
        public HttpPostedFileBase Image { get; set; }
        public string Description { get; set; }
        [DisplayName("Category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string ImageUrl { get; set; }
    }
}