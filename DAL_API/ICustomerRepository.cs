using BOL.Accounts;
using System;
using System.Collections.Generic;

namespace DAL_API
{
    public interface ICustomerRepository
    {
        //TODO should be ICustomer
        Customer FindByEmail(string email);
        Customer FindById(int? id);
        List<Customer> GetAll();
        void Remove(Customer customer);
        void Add(Customer customer);
        void Modify(Customer customer);
    }
}