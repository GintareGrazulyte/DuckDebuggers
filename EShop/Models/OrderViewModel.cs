using BOL.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShop.Models
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public OrderRating OrderRating { get; set; }
    }
}