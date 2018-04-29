using BLL_API;
using DAL_API;
using DOL;
using DOL.Accounts;
using DOL.Carts;
using DOL.Orders;
using EShop.Models;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
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

            return View(new PaymentViewModel() { Card = _card, Cart = _cart });
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

            var receiveStream = paymentResult.Content.ReadAsStreamAsync().Result;
            var readStream = new StreamReader(receiveStream, Encoding.UTF8);
            JObject responseContent = JObject.Parse(readStream.ReadToEnd());


            string paymentInfo = "";
            OrderStatus orderStatus;
 
            if (paymentResult.IsSuccessStatusCode)
            {
                orderStatus = OrderStatus.approved;
            }
            else
            {
                orderStatus = OrderStatus.waitingForPayment;

                switch (paymentResult.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        paymentInfo = "Invalid card number, ";
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        //401 nepavyko autentifikuoti API serviso vartotojo
                        //Exception
                        break;
                    case System.Net.HttpStatusCode.PaymentRequired:
                        string error = responseContent.Property("error").Value.ToString();
                        if (error == "OutOfFunds")
                        {
                            paymentInfo = "Insufficient balance in the card, ";
                        }
                        else if (error == "CardExpired")
                        {
                            paymentInfo = "Card is expired, ";
                        }
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        //404 operacija nerasta (Galima tik post)
                        //Exception
                        break;
                }
                paymentInfo += "please use another card for payment";

            }
                
            
            var currentCustomer = (Customer)Session["Account"] as Customer;
            if (currentCustomer == null)
            {
                //TODO handle;
            }

            //TODO: cannot modify as db crashes

            //var customer = _customerDAO.FindByEmail(currentCustomer.Email);
            //var order = new Order { Cart = (Cart)Session["Cart"], DateTime = DateTime.Now, OrderStatus = orderStatus };
            //customer.Orders.Add(order);
            //_customerDAO.Modify(customer);

            Session["Cart"] = null;
            Session["count"] = 0;

            return View(new PaymentViewModel() { PaymentDetails = paymentInfo, Status = orderStatus.GetDescription() });

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