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
    public class PaymentController : Controller
    {
        private IPaymentService _payment = new PaymentService();
        private Card _card;
        private Cart _cart;
        private ICustomerDAO _customerDAO;

        public PaymentController(ICustomerDAO customerDAO)
        {
            _customerDAO = customerDAO;
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

            HttpResponseMessage paymentResult = _payment.Pay(_card, _cart).Result;
            OrderStatus orderStatus;

            if (paymentResult.IsSuccessStatusCode)
            {
                orderStatus = OrderStatus.approved;
                //201 pradinis Payment objektas su papildomais id ir created_at laukais
                
            }
            else
            {
                orderStatus = OrderStatus.waitingForPayment;
 
                switch(paymentResult.StatusCode)
                {
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
                
            }
            Customer customer = _customerDAO.FindByEmail(((Customer)Session["Account"]).Email);
            customer.Order.Add(new Order { Cart = (Cart)Session["Cart"], DateTime = DateTime.Now, OrderStatus = orderStatus });

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
    }
}