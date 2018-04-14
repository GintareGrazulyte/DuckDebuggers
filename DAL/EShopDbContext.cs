using DOL.Objects;
using System.Collections.Generic;
using System.Data.Entity;

namespace DAL.EShopDbContext
{
    public class EShopDbContext : DbContext
    {
        public EShopDbContext()
            : base("DuckDebuggersEShop")
        {
        }

        public DbSet<Item> Items { get; set; }
    }
}