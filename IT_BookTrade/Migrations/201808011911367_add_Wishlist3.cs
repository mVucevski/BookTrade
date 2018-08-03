namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Wishlist3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WishlistItems", "UserEmail", c => c.String());
            DropColumn("dbo.WishlistItems", "WishlistID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WishlistItems", "WishlistID", c => c.Int(nullable: false));
            DropColumn("dbo.WishlistItems", "UserEmail");
        }
    }
}
