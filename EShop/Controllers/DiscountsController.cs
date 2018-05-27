using BLL_API;
using BOL.Discounts;
using EShop.Attributes;
using EShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class DiscountsController : Controller
    {
        private ICategoryService _categoryService;
        private IDiscountManagementService _discountManagementService;

        public DiscountsController(ICategoryService categoryService, IDiscountManagementService discountManagementService)
        {
            _categoryService = categoryService;
            _discountManagementService = discountManagementService;
        }
        // GET: Discounts
        public ActionResult Index()
        {
            return View(_discountManagementService.GetAllDiscounts());
        }


        // GET: Discounts/Create
        public ActionResult Create()
        {
            ViewData["Categories"] = _categoryService.GetAllCategories();
            return View(new DiscountViewModel() { BeginDate = DateTime.Now, EndDate = DateTime.Now });
        }

        // POST: Discounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BeginDate,EndDate,DiscountType,Value,Items")] DiscountViewModel discount)
        {
            if (ModelState.IsValid)
            {
                if (discount.Items != null)
                    discount.ItemIds = discount.Items.Split(',').Select(x => int.Parse(x)).ToList();

                Discount discountToCreate = null;
                if(discount.DiscountType == DiscountType.Absolute)
                {
                    discountToCreate = new AbsoluteDiscount()
                    {
                        AbsoluteValue = (int)discount.Value,
                        BeginDate = discount.BeginDate,
                        EndDate = discount.EndDate
                    };
                }
                else
                {
                    discountToCreate = new PercentageDiscount()
                    {
                        Percentage = discount.Value,
                        BeginDate = discount.BeginDate,
                        EndDate = discount.EndDate
                    };
                }
                _discountManagementService.CreateDiscount(discountToCreate, discount.ItemIds);

                return RedirectToAction("Index");
            }

            var view = View(discount);
            view.ViewData["Categories"] = _categoryService.GetAllCategories();
            return view;
        }

        // GET: Discounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                Discount discount = _discountManagementService.GetDiscount(id.Value);
                return View(discount);
            }
            catch (Exception e)
            {
                return HttpNotFound(e.Message);
            }
        }

        // GET: Discounts/Expirations
        public ActionResult Expirations()
        {
            return View(_discountManagementService.GetAllDiscounts().Where(x => x.EndDate < DateTime.Now).ToList());
        }
        

        public ActionResult DeleteExpired()
        {
            _discountManagementService.DeleteExpiredDiscounts();

            return RedirectToAction("Index");
        }
    }
}