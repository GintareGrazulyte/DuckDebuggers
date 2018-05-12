using BLL_API;
using BOL.Discounts;
using EShop.Attributes;
using EShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class DiscountsController : Controller
    {
        private ICategoryService _categoryService;

        public DiscountsController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        // GET: Discounts
        public ActionResult Index()
        {
            return View();
        }


        // GET: Discounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Discounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BeginDate,EndDate,DiscountType,Value")] DiscountViewModel discount)
        {
            if (ModelState.IsValid)
            {
               // _categoryService.CreateCategory(discount);
                return RedirectToAction("Index");
            }

            return View(discount);
        }


        public ActionResult TreeView()
        {
            return View(_categoryService.GetAllCategories());
        }

        public ActionResult ChooseItems(string[] items)
        {
            return View();
        }

    }
}