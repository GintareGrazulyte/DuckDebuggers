using System.Data.Entity;

namespace EShop.Models
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