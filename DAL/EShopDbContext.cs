using BOL;
using BOL.Objects;
using BOL.Accounts;
using System.Data.Entity;
using BOL.Orders;
using System.Reflection;
using BOL.Discounts;
using BOL.Carts;
using BOL.Property;

namespace DAL
{
    public class EShopDbContext : DbContext
    {
        public EShopDbContext()
            : base("DuckDebuggersEShop")
        {
        }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<OrderRating> OrderRatings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Property> Properties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Overrides for the convention-based mappings.
            // We're assuming that all our fluent mappings are declared in this assembly.
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetAssembly(typeof(EShopDbContext)));

            modelBuilder.Entity<Item>().HasKey(q => q.Id);
            modelBuilder.Entity<Property>().HasKey(q => q.Id);
            modelBuilder.Entity<ItemProperty>().HasKey(q =>
                new {
                    q.ItemId,
                    q.PropertyId
                });

            modelBuilder.Entity<ItemProperty>()
                .HasRequired(t => t.Item)
                .WithMany(t => t.ItemProperties)
                .HasForeignKey(t => t.ItemId);

            modelBuilder.Entity<ItemProperty>()
                .HasRequired(t => t.Property)
                .WithMany(t => t.ItemProperties)
                .HasForeignKey(t => t.PropertyId);
        }
        
    }
}