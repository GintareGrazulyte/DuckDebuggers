using BLL_API;
using DAL_API;
using DOL.Accounts;
using DOL.Carts;
using DOL.Orders;
using EShop.Models;
using System.Web.Mvc;
using System.Linq;
using System;

namespace EShop.Controllers
{
    public class PaymentServiceController : Controller
    {
        private IPaymentService _paymentService;
        private ICustomerDAO _customerDAO;

        public PaymentServiceController(ICustomerDAO customerDAO, IPaymentService paymentService)
        {
            _customerDAO = customerDAO;
            _paymentService = paymentService;
        }

        public ActionResult Index()
        {
            ActionResult actionResult = GetSessionProperties(out Customer customer, out Cart cart);
            if (actionResult != null)
            {
                return actionResult;
            }
            return View(new PaymentViewModel() { Customer = customer, Cart = cart, FormedOrder = false });
        }

        public ActionResult IndexFormedOrder(Cart cart)
        {
            ActionResult actionResult = GetSessionCustomer(out Customer customer);
            if (actionResult != null)
            {
                return actionResult;
            }
            return View("Index", new PaymentViewModel() { Customer = customer, Cart = cart, FormedOrder = true });
        }

        public ActionResult PayFormedOrder(Cart cart)
        {
            ActionResult actionResult = GetSessionCustomer(out Customer currentCustomer);
            if (actionResult != null)
            {
                return actionResult;
            }

            _paymentService.Payment(currentCustomer.Card, cart.Cost, out OrderStatus orderStatus, out string paymentInfo);

            //TODO: now does not change anything in DB
            var customer = _customerDAO.FindByEmail(currentCustomer.Email);
            Order order = currentCustomer.Orders.FirstOrDefault(o => o.Cart.Id == cart.Id);
            order.OrderStatus = orderStatus;
            _customerDAO.Modify(customer);

            return View("Pay", new PaymentViewModel() { PaymentDetails = paymentInfo, Status = orderStatus.GetDescription() });
        }

        //TODO add attribute
        public ActionResult Pay(Cart cart)
        {
            ActionResult actionResult = GetSessionCustomer(out Customer currentCustomer);
            if (actionResult != null)
            {
                return actionResult;
            }

            _paymentService.Payment(currentCustomer.Card, cart.Cost, out OrderStatus orderStatus, out string paymentInfo);

            //TODO: cannot modify as db crashes

            //var customer = _customerDAO.FindByEmail(currentCustomer.Email);
            //var order = new Order { Cart = (Cart)Session["Cart"], DateTime = DateTime.Now, OrderStatus = orderStatus };
            //customer.Orders.Add(order);
            //_customerDAO.Modify(customer);

            Session["Cart"] = null;
            Session["count"] = 0;

            return View(new PaymentViewModel() { PaymentDetails = paymentInfo, Status = orderStatus.GetDescription() });
        }

        private ActionResult GetSessionProperties(out Customer customer, out Cart cart)
        {
            ActionResult actionResult = GetSessionCustomer(out customer);
            cart = null;

            if (actionResult != null)
            {
                return actionResult;
            }

            if (Session["Cart"] == null)
            {
                return RedirectToAction("EmptyCart", "Cart");
            }

            cart = (Cart)Session["Cart"];

            return null;
        }

        private ActionResult GetSessionCustomer(out Customer customer)
        {
            customer = Session["Account"] as Customer;
            if (customer == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _customerDAO.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}