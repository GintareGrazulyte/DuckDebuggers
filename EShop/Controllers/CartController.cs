using BOL.Carts;
using BOL.Objects;
using BOL.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL_API;
using EShop.Attributes;

namespace EShop.Controllers
{
    public class CartController : Controller
    {
        private IItemQueryService _itemQueryService;
        private ICartService _cartService;
        private ICustomerAccountService _customerAccountService;

        public CartController(IItemQueryService itemQueryService, ICartService cartService, ICustomerAccountService customerAccountService)
        {
            _itemQueryService = itemQueryService;
            _cartService = cartService;
            _customerAccountService = customerAccountService;
        }
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["Cart"] == null || (int?)Session["Count"] == 0 )
                return RedirectToAction("EmptyCart");

            return View((Cart)Session["Cart"]);
        }

        public ActionResult EmptyCart()
        {
            return View();
        }

        public ActionResult RemoveFromCart(int? cartItemId)
        {
            Cart cart = (Cart)Session["Cart"];
            _cartService.RemoveItem(cart.Items, cartItemId);
            Session["Count"] = _cartService.CountItemsInCart(cart.Items);
            cart.Cost = cart.CountCartPrice(cart.Items);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddToCart(FormCollection fc)
        {
            int id = Convert.ToInt32(fc[0]);
            //int id = Convert.ToInt32(fc["ItemId"]);
            /*if(id == null)          //TODO: error handling
                return RedirectToAction("Index"); */

            Item item = _itemQueryService.GetItem(id);

            if(item == null)        //TODO: error handling
                return RedirectToAction("Index");

            Cart cart;
            if (Session["Cart"] == null)
            {
                cart = new Cart()
                {
                    Items = new List<CartItem>()
                };
                Session["Cart"] = cart;
            }
            else
            {
                cart = (Cart)Session["Cart"];
            }

            if ((cart.Items.FirstOrDefault(i => i.Item.Id == id) != null))
            {
                cart.Items.Where(x => x.Item.Id == id).ToList()
                    .ForEach(y => y.Quantity += 1);
            }
            else
            {
                CartItem cartItem = new CartItem()
                {
                    Cart = (Cart)Session["Cart"],
                    Item = item,
                    Quantity = 1
                };
                cart.Items.Add(cartItem);
            }

            cart.Cost = cart.CountCartPrice(cart.Items);
            
            Session["Count"] = _cartService.CountItemsInCart(cart.Items);
            return Json(new { message = item.Name + " was Added to Cart", itemCount = Session["Count"] });
        }

        public ActionResult ChangeCartItemQuantity(FormCollection fc)
        {
            if ((Cart)Session["Cart"] == null)
                return RedirectToAction("EmptyCart");

            int cartItemId = 0;
            int cartItemQuantity = 0;

            try
            {
                cartItemId = Convert.ToInt32(fc["cartItem.Item.Id"]);
                cartItemQuantity = Convert.ToInt32(fc["cartItem.Quantity"]);
                if (cartItemQuantity < 1)
                    return RedirectToAction("Index");
            }
            catch (OverflowException)
            {
                return RedirectToAction("Index");
            }
            catch(FormatException)
            {
                return RedirectToAction("Index");
            }

            Cart cart = (Cart)Session["Cart"];
            CartItem item = cart.Items.FirstOrDefault(x => x.Item.Id == cartItemId);
            if (item != null)
                item.Quantity = cartItemQuantity;

            cart.Cost = cart.CountCartPrice(cart.Items);
            Session["Count"] = _cartService.CountItemsInCart(cart.Items);
            return RedirectToAction("Index");
        }

        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult RepeatOrder(int orderId)
        {
            var customer = _customerAccountService.GetCustomer((int)Session["AccountId"]);
            Order order = customer.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return View("Index", (Cart)Session["Cart"]);
            }

            Item itemToAdd;
            foreach (var item in order.Cart.Items)
            {
                itemToAdd = _itemQueryService.GetItem(item.Item.Id);
                if (itemToAdd != null)
                {
                    FormCollection fc = new FormCollection
                    {
                        ["cartItem.Item.Id"] = Convert.ToString(itemToAdd.Id),
                        ["cartItem.Quantity"] = Convert.ToString(item.Quantity)
                    };
                    //TODO gal ne fc?
                    AddToCart(fc);
                    ChangeCartItemQuantity(fc);
                }
            }
            return View("Index", (Cart)Session["Cart"]);
        }

    }
}