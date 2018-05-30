using BOL.Objects;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public class ItemRepository : IItemRepository
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

        public ItemRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            _ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException("ambientDbContextLocator");
        }

        public void Add(Item item)
        {
            DbContext.Items.Add(item);
        }

        public Item FindById(int? id)
        {
            return DbContext.Items.Include("Category").Include("Discounts").Include("ItemProperties")
                    .SingleOrDefault(c => c.Id == id);
        }

        public List<Item> GetAll()
        {
            return DbContext.Items.Include("Discounts").Include("Category").Include("ItemProperties").ToList();
        }

        public void Modify(Item item)
        {
            DbContext.Entry(item).State = EntityState.Modified;
        }

        public void Remove(Item item)
        {
            DbContext.CartItems.Where(x => x.Item != null && x.Item.Id == item.Id).ToList()
                .ForEach(y => y.Item = null);
            DbContext.Items.Remove(item);
        }

        public List<Item> GetByIds(IEnumerable<int> ids)
        {
            return DbContext.Items.Include("Category").Include("Discounts").Where(t => ids.Contains(t.Id)).ToList();
        }
    }
}
