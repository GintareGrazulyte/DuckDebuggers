using BLL_API;
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

            RecalculatePrices(cart);
            return View(new PaymentViewModel() { Customer = customer, Cart = cart, FormedOrder = false });
        }

        public ActionResult IndexFormedOrder(int orderId)
        {
            GetSessionCustomer(out Customer customer);

            var order = customer.Orders.FirstOrDefault(o => o.Id == orderId);
            var cart = order.Cart;

            if (order == null)
            {
                //TODO handle
            }

            return View("Index", new PaymentViewModel() { Customer = customer, Cart = cart, FormedOrder = true });
        }

        public ActionResult PayFormedOrder(int cartId)
        {
            GetSessionCustomer(out Customer customer); 

            var order = customer.Orders.FirstOrDefault(o => o.Cart.Id == cartId);
            var cart = order.Cart;

            if (order == null)
            {
                //TODO handle
            }

            //Recalculation of prices is not needed as order is already formed
            var paymentInfo = _customerPaymentService.PayFormedOrder(customer.Id, cart);

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
            GetSessionCustomer(out customer);

            if (Session["Cart"] == null)
            { 
                cart = null;
                return RedirectToAction("EmptyCart", "Cart");
            }

            cart = (Cart)Session["Cart"];

            return null;
        }

        private void GetSessionCustomer(out Customer customer)  //TODO: enough to return Customer.Id?
        {
            int? customerId = (int?)Session["AccountId"];
            customer = _customerAccountService.GetCustomer((int)customerId);
        }

        private void RecalculatePrices(Cart cart)
        {
            cart.Cost = 0;

            foreach (var item in cart.Items)
            {
                if (item.Item.HasDiscount)
                {
                    item.BuyPrice = (int)(item.Item.GetPriceWithDiscount() * 100);
                }
                else
                {
                    item.BuyPrice = item.Item.Price;
                }
                cart.Cost += item.Quantity * item.BuyPrice;
            }
        }
    }
}