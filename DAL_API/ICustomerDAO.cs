using DOL.Accounts;
using System;
using System.Collections.Generic;

namespace DAL_API
{
    public interface ICustomerDAO : IDisposable
    {
        //TODO should be ICustomer
        Customer FindByEmail(string email);
        List<Customer> GetAll();
        void Remove(Customer customer);
        void Add(Customer customer);
        void Modify(Customer customer);
    }
}