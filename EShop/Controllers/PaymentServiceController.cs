using BLL_API;
using BOL;
using BOL.Accounts;
using BOL.Carts;
using BOL.Orders;
using EShop.Models;
using System.Linq;
using System.Web.Mvc;
using EShop.Attributes;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
    public class PaymentServiceController : Controller
    {
        private ICustomerPaymentService _customerPaymentService;
        private ICustomerAccountService _customerAccountService;

        public PaymentServiceController(ICustomerAccountService customerAccountService, ICustomerPaymentService customerPaymentService)
        {
            _customerAccountService = customerAccountService;
            _customerPaymentService = customerPaymentService;
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

        public ActionResult IndexFormedOrder(int orderId)
        {
            ActionResult actionResult = GetSessionCustomer(out Customer customer);
            if (actionResult != null)
            {
                return actionResult;
            }

            Order order = customer.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                //TODO handle
            }

            return View("Index", new PaymentViewModel() { Customer = customer, Cart = order.Cart, FormedOrder = true });
        }

        public ActionResult PayFormedOrder(int cartId)
        {
            ActionResult actionResult = GetSessionCustomer(out Customer customer); 

            if (actionResult != null)
            {
                return actionResult;
            }

            Order order = customer.Orders.FirstOrDefault(o => o.Cart.Id == cartId);

            if (order == null)
            {
                //TODO handle
            }

            var paymentInfo = _customerPaymentService.PayFormedOrder(customer.Id, order.Cart);

            return View("Pay", new PaymentViewModel() { PaymentInfo = paymentInfo });
        }

        public ActionResult Pay()    
        {
            ActionResult actionResult = GetSessionProperties(out Customer customer, out Cart cart);

            if (actionResult != null)
            {
                return actionResult;
            }

            var paymentInfo = _customerPaymentService.Pay(customer.Id, cart);

            Session["Cart"] = null;
            Session["Count"] = 0;

            return View(new PaymentViewModel() { PaymentInfo = paymentInfo });
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

        private ActionResult GetSessionCustomer(out Customer customer)  //TODO: enough to return Customer.Id?
        {
            customer = null; 

            int? customerId = (int?)Session["AccountId"];
            customer = _customerAccountService.GetCustomer((int)customerId);
            
            if(customer == null)
            {
                return RedirectToAction("Register", "Customer");
            }
            return null;
        }
    }
}