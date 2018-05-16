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
            int id = Convert.ToInt32(fc["itemId"]);
            int quantity = 0;
            try
            {
                quantity = Convert.ToInt32(fc["quantity"]);
            }
            catch (ArgumentOutOfRangeException)
            {
                quantity = 1;
            }
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
                    .ForEach(y => y.Quantity += quantity);
            }
            else
            {
                CartItem cartItem = new CartItem()
                {
                    Cart = (Cart)Session["Cart"],
                    Item = item,
                    Quantity = quantity
                };
                cart.Items.Add(cartItem);
            }

            cart.Cost = cart.CountCartPrice(cart.Items);
            
            Session["Count"] = _cartService.CountItemsInCart(cart.Items);

            return Json(new { message = item.Name + "(" + quantity + ")" + " was Added to Cart", itemCount = Session["Count"] });
        }

        [HttpPost]
        public ActionResult ChangeCartItemQuantity(FormCollection fc)
        {
            if ((Cart)Session["Cart"] == null)
                return RedirectToAction("EmptyCart");

            int cartItemId = 0;
            int cartItemQuantity = 0;

            try
            {
                cartItemId = Convert.ToInt32(fc["itemId"]);
                cartItemQuantity = Convert.ToInt32(fc["quantity"]);
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
            else
                return RedirectToAction("Index");

            cart.Cost = cart.CountCartPrice(cart.Items);
            Session["Count"] = _cartService.CountItemsInCart(cart.Items);
            return Json(new
            {
                cartCost = (cart.Cost / 100.0m),
                itemCount = Session["Count"],
                itemCost = (item.Item.Price*cartItemQuantity)/100.0m,
                hasDiscount = item.Item.HasDiscount,
                discountCost = ((int)item.Item.GetPriceWithDiscount()/100.0m)*cartItemQuantity,
            });
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
                        ["itemId"] = Convert.ToString(itemToAdd.Id),
                        ["quantity"] = Convert.ToString(item.Quantity)
                    };
                    AddToCart(fc);
                    ChangeCartItemQuantity(fc);
                }
            }
            return View("Index", (Cart)Session["Cart"]);
        }

    }
}