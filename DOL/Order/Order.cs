using DOL.Carts;
using System;

namespace DOL.Order
{
    public class Order
    {
        public Cart Cart { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime DateTime { get; set; }
    }
}
