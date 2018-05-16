using BOL.Accounts;
using BOL.Orders;
using System;
using System.Linq;
using System.Web.Mvc;
using BLL_API;
using EShop.Models;
using EShop.Attributes;
using System.Net;

namespace EShop.Controllers
{
    public class OrderHistoryController : Controller
    {
        private ICustomerAccountService _customerAccountService;
        private IOrderRatingService _orderRatingService;
        private IOrderService _orderService;

        public OrderHistoryController(ICustomerAccountService customerAccountService, IOrderRatingService orderRatingService, IOrderService orderService)
        {
            _customerAccountService = customerAccountService;
            _orderRatingService = orderRatingService;
            _orderService = orderService;
        }

        // GET: OrderHistory
        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult Index()
        {
            Customer currentCustomer = _customerAccountService.GetCustomer((int)Session["AccountId"]);

            if (currentCustomer.Orders.Count == 0)
                return RedirectToAction("NoOrders");
            return View(currentCustomer.Orders);
        }

        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult NoOrders()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult _OrderTable(FormCollection fc)
        {
            int orderID = 0;
            try
            {
                orderID = Convert.ToInt32(fc["OrderId"]);
            }
            catch (FormatException)
            {
                return Content("<html></html>");
            }


            int? currentCustomerId = (int)Session["AccountId"];
            if (currentCustomerId == null)
                return Content("<html></html>");

            OrderRating orderRating;
            try
            {
                orderRating = _orderRatingService.GetOrderRatingByOrderId(orderID);
            }
            catch(ArgumentException)
            {
                orderRating = null;
            }
                

            Customer currentCustomer = _customerAccountService.GetCustomer((int)currentCustomerId);
            Order order = currentCustomer.Orders.FirstOrDefault(o => o.Id == orderID);
            ViewBag.OrderId = order.Id;
            OrderViewModel ovm = new OrderViewModel { Order = order, OrderRating = orderRating };
            return View(ovm);
        }

        [HttpPost]
        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult SetRating(FormCollection form)
        {
            var comment = form["Comment"].ToString();
            var orderId = int.Parse(form["OrderId"]);
            var rating = int.Parse(form["rating"]);


            if (rating == 0)
                return Json(new { Success = "false", ErrorMsg = "Please set a rating" });
            if (comment.Equals(""))
                return Json(new { Success = "false", ErrorMsg = "Please enter a comment" });

            int? currentCustomerId = (int)Session["AccountId"];
            Customer currentCustomer = _customerAccountService.GetCustomer((int)currentCustomerId);
            Order order = currentCustomer.Orders.SingleOrDefault(o => o.Id == orderId);
            OrderRating orderRating = new OrderRating { Rating = rating, Comment = comment, Order = order};
            _orderRatingService.CreateOrderRating(orderRating, order);

            return PartialView("_OrderRatingTable", orderRating);
                
        }

        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult GetRating(FormCollection fc)
        {
            int orderID = 0;
            try
            {
                orderID = Convert.ToInt32(fc["OrderId"]);
            }
            catch (FormatException)
            {
                return Content("<html></html>");
            }

            OrderRating orderRating = _orderRatingService.GetOrderRatingByOrderId(orderID);
            return PartialView("_OrderRatingTable", orderRating);
        }

        [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
        public ActionResult AdminView()
        {
            return View(_orderService.GetAllOrders());
        }

        [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
        public ActionResult ChangeOrderStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _orderService.GetOrder(id.Value);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost]
        [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
        public ActionResult ChangeOrderStatus([Bind(Include = "Id, OrderStatus")] Order order)
        {
            _orderService.UpdateStatus(order.Id, order.OrderStatus);
            return RedirectToAction("AdminView");
        }
    }
}