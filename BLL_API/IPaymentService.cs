using DOL;
using DOL.Orders;

namespace BLL_API
{
    public interface IPaymentService
    {
        void Payment(Card card, int cost, out OrderStatus orderStatus, out string paymentInfo);
    }
}
