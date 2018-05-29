using BOL.Discounts;
using BOL.Property;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        [DisplayName("Product name")]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DisplayName("SKU code")]
        public string SKUCode { get; set; }
        //TODO maybe should be better to change from int to unsigned int?
        [Range(1, int.MaxValue, ErrorMessage = "Item price must be positive!")]
        public int Price { get; set; }
        [NotMapped]
        public HttpPostedFileBase Image { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<Discount> Discounts { get; set; }
        public ICollection<ItemProperty> ItemProperties { get; set; }

        [NotMapped]
        public bool HasDiscount
        {
            get { return Discounts.Where(x => x.EndDate >= DateTime.Now).Count() != 0; }
        }
       
        public decimal GetPriceWithDiscount()
        {
            return GetAppliedDiscount()?.CalculateDiscountedPrice(Price) ?? Price;
        }

        public Discount GetAppliedDiscount()
        {
            return HasDiscount ? Discounts.Where(x => x.EndDate >= DateTime.Now).MinBy(x => x.CalculateDiscountedPrice(Price)) : null;
        }


    }
}