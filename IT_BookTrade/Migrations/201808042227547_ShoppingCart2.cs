namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCart2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShoppingCarts", "TotalPrice", c => c.Double(nullable: false));
            AddColumn("dbo.ShoppingCarts", "DiscountCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShoppingCarts", "DiscountCode");
            DropColumn("dbo.ShoppingCarts", "TotalPrice");
        }
    }
}
