using BLL_API;
using BOL.Discounts;
using EShop.Attributes;
using EShop.Models;
using log4net;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class DiscountsController : Controller
    {
        private static ILog _logger = LogManager.GetLogger(typeof(DiscountsController));

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

                    _logger.InfoFormat("Create absolute [{0}] discount for items [{1}]. Begin date : [{2}]. End date : [{3}].", discount.Value, discount.Items, discount.BeginDate.ToString(), discount.EndDate.ToString());
                }
                else
                {
                    discountToCreate = new PercentageDiscount()
                    {
                        Percentage = discount.Value,
                        BeginDate = discount.BeginDate,
                        EndDate = discount.EndDate
                    };

                    _logger.InfoFormat("Create percentage [{0}] discount for items [{1}]. Begin date : [{2}]. End date : [{3}].", discount.Value, discount.Items, discount.BeginDate.ToString(), discount.EndDate.ToString());
                }

                _discountManagementService.CreateDiscount(discountToCreate, discount.ItemIds);

                _logger.InfoFormat("Discount sucessfully created");

                return RedirectToAction("Index");
            }

            var view = View(discount);
            view.ViewData["Categories"] = _categoryService.GetAllCategories();
            return view;
        }

        // GET: Discounts/Details/5
        public ActionResult Details(int? id)
        {
            _logger.InfoFormat("View details of discount with id [{0}]", id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                Discount discount = _discountManagementService.GetDiscount(id.Value);

                _logger.InfoFormat("Get details of discount with id [{0}] was successful", id);

                return View(discount);
            }
            catch (Exception e)
            {
                _logger.InfoFormat("Get details of discount with id [{0}] failed", id);

                return HttpNotFound(e.Message);
            }
        }

        // GET: Discounts/Expirations
        public ActionResult Expirations()
        {
            _logger.Info("Get expired discounts.");
            return View(_discountManagementService.GetAllDiscounts().Where(x => x.EndDate < DateTime.Now).ToList());
        }
        

        public ActionResult DeleteExpired()
        {
            _logger.Info("Delete expired discounts.");

            _discountManagementService.DeleteExpiredDiscounts();

            _logger.Info("Delete expired discounts was successfull.");

            return RedirectToAction("Index");
        }
    }
}