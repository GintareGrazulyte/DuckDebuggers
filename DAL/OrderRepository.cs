using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BOL.Orders;
using DAL_API;
using Mehdime.Entity;

namespace DAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;

        private EShopDbContext DbContext
        {
            get
            {
                var dbContext = _ambientDbContextLocator.Get<EShopDbContext>();

                if (dbContext == null)
                    throw new InvalidOperationException("No ambient DbContext of type EShopDbContext found");

                return dbContext;
            }
        }

        public OrderRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            _ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException("ambientDbContextLocator");
        }

        public Order FindById(int? id)
        {
            return DbContext.Orders.Include("Cart").SingleOrDefault(o => o.Id == id);
        }

        public List<Order> GetAll()
        {
            return DbContext.Orders.Include("Cart").ToList();
        }

        public void Modify(Order order)
        {
            DbContext.Entry(order).State = EntityState.Modified;
        }
    }
}
