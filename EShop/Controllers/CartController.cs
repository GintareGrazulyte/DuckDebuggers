using DAL_API;
using BOL.Accounts;
using BOL.Carts;
using BOL.Objects;
using BOL.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EShop.Controllers
{
    public class CartController : Controller
    {
        private IItemRepository _itemsDAO;
        private ICategoryRepository _categoryDAO;

        public CartController(IItemRepository itemsDAO, ICategoryRepository categoryDAO)
        {
            _itemsDAO = itemsDAO;
            _categoryDAO = categoryDAO;
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
            cart.RemoveItem(cartItemId);
            Session["Count"] = cart.CountItemsInCart();
            cart.Cost = cart.CountCartPrice();
            return RedirectToAction("Index");
        }

        public ActionResult AddToCart(int? id)
        {
            Cart cart;
            if (Session["Cart"] == null)
            {
                cart = new Cart();
                cart.Items = new List<CartItem>();
                Session["Cart"] = cart;
            }
            else
            {
                cart = (Cart)Session["Cart"];
            }
            
            Item item = _itemsDAO.Find(id);

            if ((cart.Items.FirstOrDefault(i => i.Item.Id == id) != null))
            {
                cart.Items.Where(x => x.Item.Id == id).ToList()
                    .ForEach(y => y.Quantity += 1);
            }
            else
            {
                CartItem cartItem = new CartItem();
                cartItem.Cart = (Cart)Session["Cart"];
                cartItem.Item = item;
                cartItem.Quantity = 1;
                cart.Items.Add(cartItem);
            }
            //TODO: sita gal irgi reiktu iskelt uz krepselio, kai jau zinom, kad nesikeis jis??
            cart.Items.Where(x => x.Item.Id == id).ToList()
                .ForEach(y => y.BuyPrice = y.Item.Price);
            cart.Cost = cart.CountCartPrice();
            
            Session["Count"] = cart.CountItemsInCart();
            return Redirect(Request.UrlReferrer.PathAndQuery);
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
            catch(FormatException)
            {
                return RedirectToAction("Index");
            }

            Cart cart = (Cart)Session["Cart"];
            CartItem item = cart.Items.FirstOrDefault(x => x.Item.Id == cartItemId);
            if (item != null)
                item.Quantity = cartItemQuantity;

            cart.Cost = cart.CountCartPrice();
            Session["Count"] = cart.CountItemsInCart();
            return RedirectToAction("Index");
        }

        public ActionResult RepeatOrder(int orderId)
        {
            //TODO handle exceptions
            Order order = ((Customer)Session["Account"]).Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return View("Index", (Cart)Session["Cart"]);
            }

            Item itemToAdd;
            foreach (var item in order.Cart.Items)
            {
                itemToAdd = _itemsDAO.Find(item.Item.Id);

                if (itemToAdd != null)
                {
                    AddToCart(itemToAdd.Id);
                }
            }
            return View("Index", (Cart)Session["Cart"]);
        }

    }
}