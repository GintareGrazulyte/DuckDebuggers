using DAL_API;
using BOL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CategoryRepository : ICategoryRepository
    {
        private EShopDbContext _db = new EShopDbContext();

        public void Add(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Category FindById(int? id)
        {
            return _db.Categories.Include("Items")
                    .Where(c => c.Id == id)
                    .FirstOrDefault();
        }

        public List<Category> GetAll()
        {
            return _db.Categories.Include("Items").ToList();
        }

        public void Modify(Category category)
        {
            _db.Entry(category).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(Category category)
        {
            _db.Categories.Remove(category);
            _db.SaveChanges();
        }
    }
}
