using BOL;
using BOL.Carts;

namespace BLL_API
{
    public interface ICustomerPaymentService
    {
        PaymentInfo Pay(int customerId, Cart cart);
        PaymentInfo PayFormedOrder(int customerId, Cart cart);
    }
}
