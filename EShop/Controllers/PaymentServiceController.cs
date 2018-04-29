using BLL;
using BLL_API;
using DAL_API;
using DOL;
using DOL.Accounts;
using DOL.Carts;
using DOL.Orders;
using EShop.Models;
using System;
using System.Net.Http;
using System.Web.Mvc;


namespace EShop.Controllers
{
    public class PaymentServiceController : Controller
    {
        private IPaymentService _paymentService;
        private Card _card;
        private Cart _cart;
        private ICustomerDAO _customerDAO;

        public PaymentServiceController(ICustomerDAO customerDAO, IPaymentService paymentService)
        {
            _customerDAO = customerDAO;
            _paymentService = paymentService;
        }

        //TODO add attribute
        public ActionResult Index()
        {
            ActionResult actionResult = SetSessionProperties();

            if (actionResult != null)
            {
                return actionResult;
            }

            return View(new PaymentViewModel(_cart, _card));
        }

        //TODO add attribute
        public ActionResult Pay()
        {
            ActionResult actionResult = SetSessionProperties();

            if (actionResult != null)
            {
                return actionResult;
            }

            HttpResponseMessage paymentResult = _paymentService.Pay(_card, _cart).Result;
            OrderStatus orderStatus;

            orderStatus = OrderStatus.waitingForPayment;
 
            switch(paymentResult.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    orderStatus = OrderStatus.approved;
                    //201 pradinis Payment objektas su papildomais id ir created_at laukais
                    break;
                case System.Net.HttpStatusCode.BadRequest:
                    //400 nekorektiški užklausos įvesties duomenys
                    //error obj
                    break;
                case System.Net.HttpStatusCode.Unauthorized:
                    //401 nepavyko autentifikuoti serviso vartotojo
                    break;
                case System.Net.HttpStatusCode.PaymentRequired:
                    //402 nepavyko įvykdyti mokėjimo
                    //error obj
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    //404 operacija nerasta
                    //error obj
                    break;
            }
                
            
            var currentCustomer = (Customer)Session["Account"] as Customer;
            if (currentCustomer == null)
            {
                //TODO handle;
            }

            var customer = _customerDAO.FindByEmail(currentCustomer.Email);
            customer.Orders.Add(new Order { Cart = (Cart)Session["Cart"], DateTime = DateTime.Now, OrderStatus = orderStatus });
            _customerDAO.Modify(customer);

            //TODO: reset cart 
            return View(new PaymentViewModel(orderStatus));

        }

        public ActionResult SetSessionProperties()
        {
            Customer customer = Session["Account"] as Customer;
            if (customer == null )
            {
                return RedirectToAction("Login", "Customer");
            }

            if (Session["Cart"] == null)
            {
                return RedirectToAction("EmptyCart", "Cart");
            }

            _card = ((Customer)Session["Account"]).Card;
            _cart = ((Cart)Session["Cart"]);

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