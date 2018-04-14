using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOL.Cart
{
    public class Cart
    {
        public List<CartItem> Items { get; set; }
        public decimal Cost { get; set; }
    }
}
