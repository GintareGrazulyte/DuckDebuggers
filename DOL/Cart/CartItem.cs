using DOL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOL.Cart
{
    public class CartItem
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
