﻿using BOL;
using BOL.Accounts;
using BOL.Carts;

namespace EShop.Models
{
    public class PaymentViewModel
    {
        public PaymentInfo PaymentInfo { get; set; }

        public Cart Cart { get; set; }
        public Customer Customer { get; set; }

        public bool FormedOrder { get; set; }
    }
}