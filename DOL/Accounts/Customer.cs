using BOL.Orders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BOL.Accounts
{
    public class Customer : Account
    {
        [UIHint("Card")]
        public Card Card { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
