﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL_API;
using BOL.Objects;
using System;
using Mehdime.Entity;

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
            if (ambientDbContextLocator == null) throw new ArgumentNullException("ambientDbContextLocator");
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        public void Add(Item item)
        {
            DbContext.Items.Add(item);
            DbContext.SaveChanges();
        }

        public Item Find(int? id)
        {
            return DbContext.Items.Find(id);
        }

        public List<Item> GetAll()
        {
            return DbContext.Items.ToList();
        }

        public void Modify(Item item)
        {
            DbContext.Entry(item).State = EntityState.Modified;
            DbContext.SaveChanges();
        }

        public void Remove(Item item)
        {
            DbContext.Items.Remove(item);
            DbContext.SaveChanges();
        }

    }
}
