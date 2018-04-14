using DOL.Objects;
using System.Data.Entity;

namespace DAL
{
    public class EShopDbContext : DbContext
    {
        public EShopDbContext()
            : base("DuckDebuggersEShop")
        {
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<DOL.Accounts.Customer> Customers { get; set; }
    }
}