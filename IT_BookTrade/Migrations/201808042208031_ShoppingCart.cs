namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        ShoppingCartId = c.Int(nullable: false, identity: true),
                        UserEmail = c.String(),
                    })
                .PrimaryKey(t => t.ShoppingCartId);
            
            CreateTable(
                "dbo.ShoppingCartItems",
                c => new
                    {
                        ShoppingCartItemId = c.Int(nullable: false, identity: true),
                        ShoppingCartId = c.Int(nullable: false),
                        Book_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ShoppingCartItemId)
                .ForeignKey("dbo.Books", t => t.Book_ID)
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCartId, cascadeDelete: true)
                .Index(t => t.ShoppingCartId)
                .Index(t => t.Book_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShoppingCartItems", "ShoppingCartId", "dbo.ShoppingCarts");
            DropForeignKey("dbo.ShoppingCartItems", "Book_ID", "dbo.Books");
            DropIndex("dbo.ShoppingCartItems", new[] { "Book_ID" });
            DropIndex("dbo.ShoppingCartItems", new[] { "ShoppingCartId" });
            DropTable("dbo.ShoppingCartItems");
            DropTable("dbo.ShoppingCarts");
        }
    }
}
