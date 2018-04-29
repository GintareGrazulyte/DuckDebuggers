using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOL.Carts
{
    //[ComplexType]
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ICollection<CartItem> Items { get; set; }
        public int Cost {
            get { return CountCartPrice(); }
            set { /* TODO: cia nk blogo jei tuscia???????????*/}
        }

        //TODO move to bussiness logics

        public int CountItemsInCart()
        {
            if (Items == null || Items.Count == 0)
                return 0;

            int count = 0;
            foreach(var cartItem in Items)
            {
                count += cartItem.Quantity;
            }
            return count;
        }
        private int CountCartPrice()
        {
            if (Items == null)
                return 0;
            int price = 0;
            foreach(var cartItem in Items)
            {
                price += cartItem.Item.Price * cartItem.Quantity;
            }
            return price;
        }

        public void RemoveItem(int? cartItemId)
        {
            var itemToRemove = Items.FirstOrDefault(i => i.Id == cartItemId);
            if (itemToRemove != null)
                Items.Remove(itemToRemove);
            return;
        }
    }
}
