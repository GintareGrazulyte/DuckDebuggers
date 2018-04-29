using System.Data.Entity;
using System;
using System.Net;
using System.Web.Mvc;
using DAL_API;
using DOL.Objects;
using EShop.Attributes;
using BLL_API;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class ItemController : Controller
    {
        private IItemsDAO _itemsDAO;
        private ICategoryDAO _categoryDAO;
        private IFileLoader _fileLoader;

        public ItemController(IItemsDAO itemsDAO, ICategoryDAO categoryDAO, IFileLoader fileLoader)
        {
            _itemsDAO = itemsDAO;
            _categoryDAO = categoryDAO;
            _fileLoader = fileLoader;
        }

        // GET: Item
        public ActionResult Index()
        {
            return View(_itemsDAO.GetAll());
        }

        // GET: Item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _itemsDAO.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_categoryDAO.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Description,Image,CategoryId")] Item item)
        {
            ViewBag.CategoryId = new SelectList(_categoryDAO.GetAll(), "Id", "Name");
            if (ModelState.IsValid)
            {
                item.ImageUrl = _fileLoader.Load(Server.MapPath("~/Uploads/Images"), item.Image);
                _itemsDAO.Add(item);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_categoryDAO.GetAll(), "Id", "Name", item.CategoryId);
            return View(item);
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _itemsDAO.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Description")] Item item)
        {
            if (ModelState.IsValid)
            {
                _itemsDAO.Modify(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _itemsDAO.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = _itemsDAO.Find(id);
            _itemsDAO.Remove(item);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _itemsDAO.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
