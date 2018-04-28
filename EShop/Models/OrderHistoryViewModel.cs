using DOL.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShop.Models
{
    public class OrderHistoryViewModel
    {
        public Order Order { get; set; }
        public Cart Cart { get; set; }
    }
}