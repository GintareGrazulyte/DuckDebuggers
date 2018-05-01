using BOL.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface ICustomerAccountService
    {
        void CreateCustomer(Customer customerToCreate);
        Customer LoginCustomer(Customer customerToLogin);
    }
}
