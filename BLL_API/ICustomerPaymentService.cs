using BOL;
using BOL.Accounts;
using BOL.Carts;

namespace BLL_API
{
    public interface ICustomerPaymentService
    {
        PaymentInfo Pay(int customerId, Cart cart, string cvv);
        PaymentInfo PayFormedOrder(int customerId, Cart cart, string cvv);
    }
}
