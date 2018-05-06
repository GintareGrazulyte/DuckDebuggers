using BLL_API;
using BOL;
using BOL.Carts;
using BOL.Orders;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Linq;

namespace BLL
{
    public class CustomerPaymentService : ICustomerPaymentService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly ICustomerRepository _customerRepository;
        private readonly IItemQueryService _itemQueryService;
        private readonly IPaymentService _paymentService;

        public CustomerPaymentService(IDbContextScopeFactory dbContextScopeFactory, ICustomerRepository customerRepository,
                                        IPaymentService paymentService, IItemQueryService itemQueryService)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _customerRepository = customerRepository ?? throw new ArgumentNullException("customerRepository");
            _paymentService = paymentService ?? throw new ArgumentNullException("paymentService");
            _itemQueryService = itemQueryService ?? throw new ArgumentNullException("itemQueryService");
        }

        public PaymentInfo Pay(int customerId, Cart cart)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var customer = _customerRepository.FindById(customerId);
                if (customer == null)
                {
                    //TODO: CustomerNotFoundException
                    throw new Exception();
                }
                var paymentInfo = _paymentService.Payment(customer.Card, cart.Cost);

                cart.Items.ToList().ForEach(x => x.Item = _itemQueryService.GetItem(x.Item.Id)); //TODO: remove hack
                
                var order = new Order { Cart = cart, DateTime = DateTime.Now, OrderStatus = paymentInfo.OrderStatus };
                customer.Orders.Add(order);
                _customerRepository.Modify(customer);

                dbContextScope.SaveChanges();
                return paymentInfo;
            }        
        }

        public PaymentInfo PayFormedOrder(int customerId, Cart cart)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var customer = _customerRepository.FindById(customerId);
                if (customer == null)
                {
                    //TODO: CustomerNotFoundException
                    throw new Exception();
                }
                var paymentInfo = _paymentService.Payment(customer.Card, cart.Cost);

                Order order = customer.Orders.FirstOrDefault(o => o.Cart.Id == cart.Id);
                order.OrderStatus = paymentInfo.OrderStatus;
                _customerRepository.Modify(customer);

                dbContextScope.SaveChanges();
                return paymentInfo;
            }
        }
    }
}
