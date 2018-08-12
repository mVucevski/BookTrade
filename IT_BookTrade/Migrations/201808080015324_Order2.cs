namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "ShippingAddress", c => c.String());
            AddColumn("dbo.OrderDetails", "BillingAddress", c => c.String());
            AddColumn("dbo.OrderDetails", "CardName", c => c.String());
            AddColumn("dbo.OrderDetails", "CardNumber", c => c.String());
            AddColumn("dbo.OrderDetails", "ExpDate", c => c.String());
            AddColumn("dbo.OrderDetails", "Gift", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "Gift");
            DropColumn("dbo.OrderDetails", "ExpDate");
            DropColumn("dbo.OrderDetails", "CardNumber");
            DropColumn("dbo.OrderDetails", "CardName");
            DropColumn("dbo.OrderDetails", "BillingAddress");
            DropColumn("dbo.OrderDetails", "ShippingAddress");
        }
    }
}
