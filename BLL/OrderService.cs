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
    public class OrderService : IOrderService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IDbContextScopeFactory dbContextScopeFactory, IOrderRepository orderRatingRepository)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _orderRepository = orderRatingRepository ?? throw new ArgumentNullException("orderRatingRepository");
        }

        public IEnumerable<Order> GetAllOrders()
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                return _orderRepository.GetAll();
            }
        }

        public Order GetOrder(int orderId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                return _orderRepository.FindById(orderId);
            }
        }

        public void UpdateStatus(int orderId, OrderStatus status)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var foundOrder = _orderRepository.FindById(orderId);
                if (foundOrder == null)
                {
                    throw new Exception();
                }

                foundOrder.OrderStatus = status;

                _orderRepository.Modify(foundOrder);
                dbContextScope.SaveChanges();
            }
        }
    }
}
