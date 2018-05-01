using BOL;
using BOL.Orders;

namespace BLL_API
{
    public interface IPaymentService
    {
        void Payment(Card card, int cost, out OrderStatus orderStatus, out string paymentInfo);
    }
}
