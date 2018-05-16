using BOL.Discounts;
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
    public class DiscountRepository : IDiscountRepository
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

        public DiscountRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            _ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException("ambientDbContextLocator");
        }

        public void Add(Discount discount)
        {
            DbContext.Discounts.Add(discount);
        }

        public Discount FindById(int? id)
        {
            return DbContext.Discounts.Include("Items")
                            .SingleOrDefault(c => c.Id == id);
        }

        public List<Discount> GetAll()
        {
            return DbContext.Discounts.ToList();
        }

        public void Modify(Discount discount)
        {
            DbContext.Entry(discount).State = EntityState.Modified;
        }

        public void Remove(Discount discount)
        {
            DbContext.Discounts.Remove(discount);
        }
    }
}
