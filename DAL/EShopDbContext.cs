using DOL;
using DOL.Objects;
using DOL.Accounts;
using System.Data.Entity;
using DOL.Orders;

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

        public DbSet<Order> Orders { get; set; }
    }
}