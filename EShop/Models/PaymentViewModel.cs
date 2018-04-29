using DOL;
using DOL.Carts;

namespace EShop.Models
{
    public class PaymentViewModel
    {
        public string Status { get; set; }
        public string PaymentDetails { get; set; }

        public Cart Cart;
        public Card Card;
    }
}