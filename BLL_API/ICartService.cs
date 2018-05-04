using BOL.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface ICartService
    {
        int CountItemsInCart(ICollection<CartItem> Items);
        int CountCartPrice(ICollection<CartItem> Items);
        void RemoveItem(ICollection<CartItem> Items, int? cartItemId);
    }
}
