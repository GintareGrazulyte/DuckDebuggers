using BOL.Property;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PropertyRepository : IPropertyRepository
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

        public PropertyRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            _ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException("ambientDbContextLocator");
        }

        public void Add(Property property)
        {
            DbContext.Properties.Add(property);
        }

        public Property FindById(int id)
        {
            return DbContext.Properties.Include("ItemProperties")
                    .SingleOrDefault(c => c.Id == id);
        }

        public List<Property> GetAll()
        {
            return DbContext.Properties.Include("ItemProperties").ToList();
        }

        public void Modify(Property property)
        {
            DbContext.Entry(property).State = EntityState.Modified;
        }

        public void Remove(Property property)
        {
            DbContext.Properties.Remove(property);
        }
    }
}
