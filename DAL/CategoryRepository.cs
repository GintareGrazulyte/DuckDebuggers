using DAL_API;
using BOL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mehdime.Entity;

namespace DAL
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;

        private EShopDbContext DbContext
        {
            get
            {
                var dbContext = _ambientDbContextLocator.Get<EShopDbContext>();

                if (dbContext == null)
                    throw new InvalidOperationException("No ambient DbContext of type EShopDbContext found");

                return dbContext;
            }
        }

        public CategoryRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (ambientDbContextLocator == null) throw new ArgumentNullException("ambientDbContextLocator");
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        public void Add(Category category)
        {
            DbContext.Categories.Add(category);
        }

        public Category FindById(int? id)
        {
            return DbContext.Categories.Include("Items")
                    .SingleOrDefault(c => c.Id == id);
        }

        public List<Category> GetAll()
        {
            return DbContext.Categories.Include("Items").ToList();
        }

        public void Modify(Category category)
        {
            DbContext.Entry(category).State = EntityState.Modified;
        }

        public void Remove(Category category)
        {
            DbContext.Categories.Remove(category);
        }

        public Category FindByName(string name)
        {
            return DbContext.Categories.Include("Items")
                    .Where(c => c.Name == name)
                    .SingleOrDefault();
        }
    }
}
