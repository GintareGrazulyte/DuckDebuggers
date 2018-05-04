using BOL;
using BOL.Orders;

namespace BLL_API
{
    public interface IPaymentService
    {
        PaymentInfo Payment(Card card, int cost);
    }
}
