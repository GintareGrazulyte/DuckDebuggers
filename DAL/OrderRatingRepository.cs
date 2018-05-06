using DAL_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOL.Orders;
using Mehdime.Entity;

namespace DAL
{
    public class OrderRatingRepository : IOrderRatingRepository
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

        public OrderRatingRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            _ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException("ambientDbContextLocator");
        }

        

        public void Add(OrderRating orderRating)
        {
            DbContext.OrderRatings.Add(orderRating);
        }

        public OrderRating FindById(int? id)
        {
            return DbContext.OrderRatings.Include("Order")
                .SingleOrDefault(o => o.Id == id);
        }

        public OrderRating FindByOrderId(int? id)
        {
            return DbContext.OrderRatings.Include("Order")
                .SingleOrDefault(o => o.Order.Id == id);
        }

        public List<OrderRating> GetAll()
        {
            return DbContext.OrderRatings.ToList();
        }

        public void Remove(OrderRating orderRating)
        {
            DbContext.OrderRatings.Remove(orderRating);
        }

        
    }
}
