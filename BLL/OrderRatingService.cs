using BLL_API;
using BOL.Accounts;
using BOL.Carts;
using BOL.Orders;
using DAL;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;

namespace BLL
{
    //todo: interface
    public class OrderRatingService : IOrderRatingService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IOrderRatingRepository _orderRatingRepository;

        public OrderRatingService(IDbContextScopeFactory dbContextScopeFactory, IOrderRatingRepository orderRatingRepository)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _orderRatingRepository = orderRatingRepository ?? throw new ArgumentNullException("orderRatingRepository");
        }

        public void CreateOrderRating(OrderRating orderRatingToCreate, Order order)
        {
            if (orderRatingToCreate == null)
                throw new ArgumentNullException("orderRatingToCreate");

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundOrderRating = _orderRatingRepository.FindByOrderId(orderRatingToCreate.Order.Id);
                if (foundOrderRating != null)
                {
                    //TODO: OrderRatingAlreadyExistsException
                    throw new Exception();
                }

                //NO HACK
                var context = dbContextScope.DbContexts.Get<EShopDbContext>();
                context.Orders.Attach(order);
                orderRatingToCreate.Order = order;

                _orderRatingRepository.Add(orderRatingToCreate);
                
                dbContextScope.SaveChanges();
            }
        }

        public void DeleteOrderRating(int orderRatingId)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundOrderRating = _orderRatingRepository.FindById(orderRatingId);
                if (foundOrderRating == null)
                {
                    //TODO: OrderRatingNotFoundException
                    throw new Exception();
                }

                _orderRatingRepository.Remove(foundOrderRating);
                dbContextScope.SaveChanges();
            }
        }

        public IEnumerable<OrderRating> GetAllOrderRatings()
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                return _orderRatingRepository.GetAll();
            }
        }

        public OrderRating GetOrderRating(int orderRatingId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                OrderRating orderRating = _orderRatingRepository.FindById(orderRatingId);

                if (orderRating == null)
                    throw new ArgumentException(String.Format("Invalid value provided for orderRatingId: [{0}].", orderRatingId));

                return orderRating;
            }
        }

        public OrderRating GetOrderRatingByOrderId(int orderId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                OrderRating orderRating = _orderRatingRepository.FindByOrderId(orderId);

                if (orderRating == null)
                    throw new ArgumentException(String.Format("Invalid value provided for orderId: [{0}].", orderId));

                return orderRating;
            }
        }
    }
}
