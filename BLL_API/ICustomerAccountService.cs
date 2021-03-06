﻿using BOL.Accounts;
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
        Customer GetCustomer(int customerId);
        Customer GetCustomer(string customerEmail);
        void Modify(Customer customer);
        void UpdatePassword(int customerId, string newPassword);
        List<Customer> GetCustomers();
        void ChangeStatus(Customer account);
    }
}
