using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BOL.Property;
using System.Web.Mvc;
using BLL_API;

namespace EShop.Controllers
{
    public class PropertyController : Controller
    {
        readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        public ActionResult Index()
        {
            return View(_propertyService.GetAllProperties());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Property property)
        {
            if (ModelState.IsValid)
            {
                _propertyService.AddProperty(property.Name);
                return RedirectToAction("Index");
            }
            return View(property);
        }
    }
}