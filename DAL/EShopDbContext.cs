using BOL;
using BOL.Objects;
using BOL.Accounts;
using System.Data.Entity;
using BOL.Carts;

namespace DAL
{
    public class EShopDbContext : DbContext
    {
        public EShopDbContext()
            : base("DuckDebuggersEShop")
        {
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Admin> Admins { get; set; }

    }
}