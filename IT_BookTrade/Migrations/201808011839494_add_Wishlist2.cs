namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Wishlist2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WishlistItems", "WishlistID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WishlistItems", "WishlistID");
        }
    }
}
