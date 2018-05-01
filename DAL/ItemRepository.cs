using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL_API;
using BOL.Objects;

namespace DAL
{
    public class ItemRepository : IItemRepository
    {
        private EShopDbContext _db = new EShopDbContext();

        public void Add(Item item)
        {
            _db.Items.Add(item);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Item Find(int? id)
        {
            return _db.Items.Find(id);
        }

        public List<Item> GetAll()
        {
            return _db.Items.ToList();
        }

        public void Modify(Item item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(Item item)
        {
            _db.Items.Remove(item);
            _db.SaveChanges();
        }

    }
}
