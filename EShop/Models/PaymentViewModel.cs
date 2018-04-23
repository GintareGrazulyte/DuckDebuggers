using DOL;
using DOL.Carts;
using DOL.Orders;

namespace EShop.Models
{
    public class PaymentViewModel
    {
        public string Status { get; set; }

        public Cart Cart;

        public Card Card;

        public PaymentViewModel(OrderStatus status)
        {
            Status = status.ToString();
        }

        public PaymentViewModel(Cart cart, Card card)
        {
            Cart = cart;
            Card = card;
        }

    }
}