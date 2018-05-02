using BOL.Accounts;
using BOL.Carts;

namespace EShop.Models
{
    public class PaymentViewModel
    {
        public string Status { get; set; }
        public string PaymentDetails { get; set; }

        public Cart Cart { get; set; }
        public Customer Customer;

        public bool FormedOrder;
    }
}