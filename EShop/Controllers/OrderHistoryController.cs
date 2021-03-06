﻿using BOL.Accounts;
using BOL.Orders;
using System;
using System.Linq;
using System.Web.Mvc;
using BLL_API;
using EShop.Models;
using EShop.Attributes;
using System.Net;
using log4net;

namespace EShop.Controllers
{
    public class OrderHistoryController : Controller
    {
        private static ILog _logger = LogManager.GetLogger(typeof(OrderHistoryController));

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
            _logger.Info("Get all orders");

            Customer currentCustomer = _customerAccountService.GetCustomer((int)Session["AccountId"]);

            _logger.InfoFormat("All orders count : [{0}]", currentCustomer.Orders.Count);

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

            _logger.InfoFormat("Add rating for oder with id [{0}]. Comment : [{1}], rating : [{2}]", orderId, comment, rating);

            if (rating == 0)
                return Json(new { Success = "false", ErrorMsg = "Please set a rating" });
            if (comment.Equals(""))
                return Json(new { Success = "false", ErrorMsg = "Please enter a comment" });

            int? currentCustomerId = (int)Session["AccountId"];
            Customer currentCustomer = _customerAccountService.GetCustomer((int)currentCustomerId);
            Order order = currentCustomer.Orders.SingleOrDefault(o => o.Id == orderId);
            OrderRating orderRating = new OrderRating { Rating = rating, Comment = comment, Order = order};
            _orderRatingService.CreateOrderRating(orderRating, order);

            _logger.InfoFormat("Rating for order with id [{0}] was successfully added", orderId);

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

            _logger.InfoFormat("Get rating for order with id [{0}]", orderID);

            OrderRating orderRating = _orderRatingService.GetOrderRatingByOrderId(orderID);

            _logger.InfoFormat("Get rating for order with id [{0}] was successful", orderID);

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
            _logger.InfoFormat("Change order with id [{0}] status [{1}].", order.Id, order.OrderStatus);
            _orderService.UpdateStatus(order.Id, order.OrderStatus);

            _logger.InfoFormat("Change order with id [{0}] to status [{1}] was successful.", order.Id, order.OrderStatus);

            return RedirectToAction("AdminView");
        }
    }
}