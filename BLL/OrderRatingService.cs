using BLL_API;
using BOL.Accounts;
using BOL.Orders;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    //todo: interface
    public class OrderRatingService : IOrderRatingService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IOrderRatingRepository _orderRatingRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrderRatingService(IDbContextScopeFactory dbContextScopeFactory, IOrderRatingRepository orderRatingRepository)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _orderRatingRepository = orderRatingRepository ?? throw new ArgumentNullException("orderRatingRepository");
        }

        public void CreateOrderRating(OrderRating orderRatingToCreate)
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
