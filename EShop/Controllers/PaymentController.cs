using BLL;
using BLL_API;
using DOL;
using DOL.Accounts;
using DOL.Carts;
using DOL.Orders;
using EShop.Models;
using System;
using System.Collections.ObjectModel;
using System.Web.Mvc;


namespace EShop.Controllers
{
    public class PaymentController : Controller
    {
        private IPayment _payment = new Payment();
        private Card _card;
        private Cart _cart;


        public ActionResult Index()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            SetSessionProperties();

            return View(new PaymentViewModel(_cart, _card));
        }

        public ActionResult Pay()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            if (Session["Cart"] == null)
            {
                return RedirectToAction("EmptyCart", "Cart");
            }

            SetSessionProperties();

            bool paymentResult = _payment.Pay(_card, _cart).Result;
            OrderStatus orderStatus;

            if (paymentResult)
            {
                orderStatus = OrderStatus.approved;
                //TODO: Form order with status approved
            }
            else
            {
                orderStatus = OrderStatus.waitingForPayment;
                //TODO: Form order with status waitingForPayment
            }

            return View(new PaymentViewModel(orderStatus));

        }

        public void SetSessionProperties()
        {        
            if(Session["Account"] != null)
            {
                _card = ((Customer)Session["Account"]).Card;
                
            }

            _cart = ((Cart)Session["Cart"]);

            //TODO: delete mock
            //_card = new Card { CVV = 123, ExpMonth = 9, ExpYear = 2018, Holder = "Test", Number = "4111111111111111" };
            _cart = new Cart { Id = 0, Cost = 123, Items = new Collection<CartItem>() };
        }
    }
}