using BLL_API;
using BOL.Accounts;
using BOL.Carts;
using BOL.Orders;
using EShop.Models;
using System.Web.Mvc;

namespace EShop.Controllers
{
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

        public ActionResult IndexFormedOrder(Cart cart)
        {
            ActionResult actionResult = GetSessionCustomer(out Customer customer);
            if (actionResult != null)
            {
                return actionResult;
            }
            return View("Index", new PaymentViewModel() { Customer = customer, Cart = cart, FormedOrder = true });
        }

        public ActionResult PayFormedOrder()
        {

            ActionResult actionResult = GetSessionProperties(out Customer customer, out Cart cart); 

            if (actionResult != null)
            {
                return actionResult;
            }

            var paymentInfo = _customerPaymentService.PayFormedOrder(customer.Id, cart);

            //TODO: is this needed?
            Session["Cart"] = null;
            Session["Count"] = 0;

            return View("Pay", new PaymentViewModel() { PaymentDetails = paymentInfo.PaymentDetails, Status = paymentInfo.OrderStatus.GetDescription() });
        }

        //TODO add attribute
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

            return View(new PaymentViewModel() { PaymentDetails = paymentInfo.PaymentDetails, Status = paymentInfo.OrderStatus.GetDescription() });
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

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            customer = _customerAccountService.GetCustomer((int)customerId);
            
            if(customer == null)
            {
                return RedirectToAction("Register", "Customer");
            }
            return null;
        }
    }
}