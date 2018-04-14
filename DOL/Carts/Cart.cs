using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOL.Carts
{
    public class Cart
    {
        public ICollection<CartItem> Items { get; set; }
        public int Cost { get; set; }
    }
}
