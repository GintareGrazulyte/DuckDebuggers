using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOL.Accounts
{
    public class Customer : Account
    {
        public Card Card { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
