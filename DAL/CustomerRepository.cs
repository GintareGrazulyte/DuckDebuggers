using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL_API;
using BOL.Accounts;
using System;
using Mehdime.Entity;

namespace DAL
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;

        private EShopDbContext DbContext
        {
            get
            {
                var dbContext = _ambientDbContextLocator.Get<EShopDbContext>();

                if (dbContext == null)
                    throw new InvalidOperationException("No ambient DbContext of type EShopDbContext found.");

                return dbContext;
            }
        }

        public CustomerRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (ambientDbContextLocator == null) throw new ArgumentNullException("ambientDbContextLocator");
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        public void Add(Customer customer)
        {
            DbContext.Customers.Add(customer);
            DbContext.SaveChanges();
        }

        public Customer FindByEmail(string email)
        {
            return DbContext.Customers.Include("Orders.Cart.Items.Item.Category")
                    .Where(c => c.Email == email)
                    .FirstOrDefault();
        }

        public Customer FindById(int? id)
        {
            if (id == null)
                return null;

            return DbContext.Customers
                    .Where(c => c.Id == id)
                    .FirstOrDefault();
        }


        public List<Customer> GetAll()
        {
            return DbContext.Customers.Include("Orders.Cart.Items.Item.Category").ToList();
        }

        public void Modify(Customer customer)
        {
            DbContext.Entry(customer).State = EntityState.Modified;
            DbContext.SaveChanges();
          
        }

        public void Remove(Customer customer)
        {
            DbContext.Customers.Remove(customer);
            DbContext.SaveChanges();
        }

    }
}
