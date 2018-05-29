using BOL.Property;
using System.Web.Mvc;
using BLL_API;
using EShop.Attributes;
using System.Net;
using log4net;
using System;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class PropertyController : Controller
    {
        private static ILog _logger = LogManager.GetLogger(typeof(PropertyController));

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
            _logger.InfoFormat("Create property with name [{0}]", property.Name);

            if (ModelState.IsValid)
            {
                _propertyService.AddProperty(property.Name);

                _logger.InfoFormat("Create property with name [{0}] was successful.", property.Name);
                return RedirectToAction("Index");
            }
            return View(property);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                Property category = _propertyService.GetProperty(id.Value);
                return View(category);
            }
            catch (ArgumentException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Property property)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _propertyService.Update(property);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(property);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                Property property = _propertyService.GetProperty(id.Value);
                return View(property);
            }
            catch (ArgumentException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _propertyService.Delete(id);
                return RedirectToAction("Index");
            }
            catch (ArgumentException)
            {
                return HttpNotFound();
            }
        }
    }
}