using BOL.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class PaymentInfo
    {
        public OrderStatus OrderStatus { get; set; }
        public string PaymentDetails { get; set; }
    }
}
