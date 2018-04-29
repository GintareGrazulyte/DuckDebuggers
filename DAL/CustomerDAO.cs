using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL_API;
using DOL.Accounts;
using System;

namespace DAL
{
    public class CustomerDAO : ICustomerDAO
    {
        private EShopDbContext _db = new EShopDbContext();

        public void Add(Customer customer)
        {
            _db.Customers.Add(customer);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Customer FindByEmail(string email)
        {
            return _db.Customers.Include("Orders.Cart.Items.Item.Category")
                    .Where(c => c.Email == email)
                    .FirstOrDefault();
        }

        public List<Customer> GetAll()
        {
            return _db.Customers.Include("Orders.Cart.Items.Item.Category").ToList();
        }

        public void Modify(Customer customer)
        {
            _db.Entry(customer).State = EntityState.Modified;
            _db.SaveChanges();
          
        }

        public void Remove(Customer customer)
        {
            _db.Customers.Remove(customer);
            _db.SaveChanges();
        }

    }
}
