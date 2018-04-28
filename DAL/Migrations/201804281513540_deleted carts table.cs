namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedcartstable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CartItems", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.CartItems", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts");
            DropIndex("dbo.Orders", new[] { "Cart_Id" });
            DropIndex("dbo.CartItems", new[] { "Cart_Id" });
            DropIndex("dbo.CartItems", new[] { "Item_Id" });
            AddColumn("dbo.Orders", "Cart_Cost", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "Cart_Id");
            DropTable("dbo.Carts");
            DropTable("dbo.CartItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Cart_Id = c.Int(),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "Cart_Id", c => c.Int());
            DropColumn("dbo.Orders", "Cart_Cost");
            CreateIndex("dbo.CartItems", "Item_Id");
            CreateIndex("dbo.CartItems", "Cart_Id");
            CreateIndex("dbo.Orders", "Cart_Id");
            AddForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts", "Id");
            AddForeignKey("dbo.CartItems", "Item_Id", "dbo.Items", "Id");
            AddForeignKey("dbo.CartItems", "Cart_Id", "dbo.Carts", "Id");
        }
    }
}
