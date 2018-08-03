namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Wishlist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishlistItems",
                c => new
                    {
                        WishlistItemId = c.Int(nullable: false, identity: true),
                        DateAdded = c.DateTime(nullable: false),
                        Book_ID = c.Int(),
                    })
                .PrimaryKey(t => t.WishlistItemId)
                .ForeignKey("dbo.Books", t => t.Book_ID)
                .Index(t => t.Book_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishlistItems", "Book_ID", "dbo.Books");
            DropIndex("dbo.WishlistItems", new[] { "Book_ID" });
            DropTable("dbo.WishlistItems");
        }
    }
}
