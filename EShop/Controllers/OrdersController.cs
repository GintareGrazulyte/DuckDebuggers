using DAL;
using DOL.Orders;
using System.Net;
using System.Web.Mvc;

namespace EShop.Controllers
{
    //[CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class OrdersController : Controller
    {
        private OrderDAO _ordersDAO;

        public OrdersController(OrderDAO ordersDAO)
        {
            _ordersDAO = ordersDAO;
        }

        // GET: Order
        public ActionResult Index()
        {
            return View(_ordersDAO.GetAll());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order item = _ordersDAO.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList("Id", "Name");
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Description,CategoryId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _ordersDAO.Add(order);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList("Id", "Name");
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order item = _ordersDAO.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = _ordersDAO.Find(id);
            _ordersDAO.Remove(order);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ordersDAO.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}