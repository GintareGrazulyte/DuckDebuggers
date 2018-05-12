using BOL.Accounts;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;

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
            _ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException("ambientDbContextLocator");
        }

        public void Add(Customer customer)
        {
            DbContext.Customers.Add(customer);
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

            return DbContext.Customers.Include("Orders.Cart.Items.Item.Category")
                    .Where(c => c.Id == id)
                    .FirstOrDefault();
        }

        public List<Customer> GetAll()
        {
            return DbContext.Customers.Include("Orders.Cart.Items.Item.Category").ToList();
        }

        public List<Customer> GetAll(string keyWord)
        {
            if (keyWord == null)
            {
                return new List<Customer>();
            }
            Regex good = new Regex(@"" + keyWord + "", RegexOptions.IgnoreCase);
            return DbContext.Customers.Where((x => good.IsMatch(x.Name) || good.IsMatch(x.Surname)
                                            || good.IsMatch(x.Email))).Distinct().ToList();
        }

        public void Modify(Customer customer)
        {
            DbContext.Entry(customer).State = EntityState.Modified;
        }

        public void UpdateCustomerOrder(Customer customer)
        {
            DbContext.Entry(customer).State = EntityState.Modified;
        }

        public void Remove(Customer customer)
        {
            DbContext.Customers.Remove(customer);
        }

    }
}
