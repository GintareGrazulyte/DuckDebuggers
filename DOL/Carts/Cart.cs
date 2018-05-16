using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL.Carts
{
    //[ComplexType]
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ICollection<CartItem> Items { get; set; }
        public int Cost { get; set; }

        public int CountCartPrice(ICollection<CartItem> Items)
        {
            if (Items == null)
                return 0;
            int price = 0;
            foreach (var cartItem in Items)
            {
                if (cartItem.Item.HasDiscount)
                {
                    //apvalinimas
                    price += (int)cartItem.Item.GetPriceWithDiscount() * cartItem.Quantity;
                }
                else
                {
                    price += cartItem.Item.Price * cartItem.Quantity;
                }
            }
            return price;
        }
    }
}
