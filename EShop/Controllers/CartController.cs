using DAL_API;
using DOL.Carts;
using DOL.Objects;
using EShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShop.Controllers
{
    public class CartController : Controller
    {
        private IItemsDAO _itemsDAO;
        private ICategoryDAO _categoryDAO;
        //private ICollection<CartItem> _cartItems;

        public CartController(IItemsDAO itemsDAO, ICategoryDAO categoryDAO)
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
            Session["Count"] = cart.Items.Count;
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
                //ViewBag.cart = cartItems.Count();
                //Session["Count"] = 1;
            }
            else
            {
                cart = (Cart)Session["Cart"];
            }
            
            Item item = _itemsDAO.Find(id);

            if ((cart.Items.FirstOrDefault(i => i.Item.Id == id) != null))
            {
                //TODO: PAKEIST I LINQ 
                foreach(CartItem cItem in cart.Items)
                {
                    if (cItem.Item.Id == id)
                        cItem.Quantity++;
                }
            }
            else
            {
                CartItem cartItem = new CartItem();
                cartItem.Cart = (Cart)Session["Cart"];
                cartItem.Item = item;
                cartItem.Quantity = 1;
                cart.Items.Add(cartItem);
            }
            Session["Count"] = cart.Items.Count;
            return RedirectToAction("Index", "Item");
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
            return RedirectToAction("Index");
        }

    }
}