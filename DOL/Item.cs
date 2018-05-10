using BOL.Discounts;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace BOL.Objects
{
    public class Item
    {
        public Item()
        {
            Discounts = new HashSet<Discount>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //TODO maybe should be better to change from int to unsigned int?
        [Range(1, int.MaxValue, ErrorMessage = "Item price must be positive!")]
        public int Price { get; set; }
        [NotMapped]
        public HttpPostedFileBase Image { get; set; }
        public string Description { get; set; }
        [DisplayName("Category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<Discount> Discounts { get; set; }
    }
}