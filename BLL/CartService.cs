using BLL_API;
using BOL.Carts;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class CartService : ICartService
    {
        public int CountItemsInCart(ICollection<CartItem> Items)
        {
            if (Items == null || Items.Count == 0)
                return 0;

            int count = 0;
            foreach (var cartItem in Items)
            {
                count += cartItem.Quantity;
            }
            return count;
        }


        public void RemoveItem(ICollection<CartItem> Items, int? cartItemId)
        {
            var itemToRemove = Items.FirstOrDefault(i => i.Id == cartItemId);
            if (itemToRemove != null)
                Items.Remove(itemToRemove);
            return;
        }
    }
}
