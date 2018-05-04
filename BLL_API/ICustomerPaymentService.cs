using BOL;
using BOL.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface ICustomerPaymentService
    {
        PaymentInfo Pay(int customerId, Cart cart);
        PaymentInfo PayFormedOrder(int customerId, Cart cart);
    }
}
