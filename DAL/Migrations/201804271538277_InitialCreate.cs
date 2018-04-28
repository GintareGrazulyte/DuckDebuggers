namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Int(nullable: false),
                        Description = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Card_Number = c.String(nullable: false, maxLength: 16),
                        Card_Holder = c.String(nullable: false),
                        Card_ExpYear = c.Int(nullable: false),
                        Card_ExpMonth = c.Int(nullable: false),
                        Card_CVV = c.String(nullable: false, maxLength: 3),
                        DeliveryAddress_StreetNumber = c.Int(nullable: false),
                        DeliveryAddress_Country = c.String(nullable: false),
                        DeliveryAddress_Street = c.String(nullable: false),
                        DeliveryAddress_City = c.String(nullable: false),
                        DeliveryAddress_Apartment = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderStatus = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Cart_Id = c.Int(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carts", t => t.Cart_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Cart_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Cart_Id = c.Int(),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carts", t => t.Cart_Id)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .Index(t => t.Cart_Id)
                .Index(t => t.Item_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.CartItems", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.CartItems", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.Items", "CategoryId", "dbo.Categories");
            DropIndex("dbo.CartItems", new[] { "Item_Id" });
            DropIndex("dbo.CartItems", new[] { "Cart_Id" });
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
            DropIndex("dbo.Orders", new[] { "Cart_Id" });
            DropIndex("dbo.Items", new[] { "CategoryId" });
            DropTable("dbo.CartItems");
            DropTable("dbo.Carts");
            DropTable("dbo.Orders");
            DropTable("dbo.Customers");
            DropTable("dbo.Items");
            DropTable("dbo.Categories");
            DropTable("dbo.Admins");
        }
    }
}
