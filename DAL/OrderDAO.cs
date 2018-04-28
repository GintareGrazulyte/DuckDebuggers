using DAL_API;
using DOL.Orders;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public class OrderDAO : IOrderDAO
    {
        private EShopDbContext _db = new EShopDbContext();

        public void Add(Order order)
        {
            _db.Orders.Add(order);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Order Find(int? id)
        {
            return _db.Orders.Find(id);
        }

        public List<Order> GetAll()
        {
            return _db.Orders.ToList();
        }

        public void Modify(Order order)
        {
            _db.Entry(order).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(Order order)
        {
            _db.Orders.Remove(order);
            _db.SaveChanges();
        }
    }
}
