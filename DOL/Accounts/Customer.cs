using BOL.Orders;
using System.Collections.Generic;

namespace BOL.Accounts
{
    public class Customer : Account
    {
        public Card Card { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
