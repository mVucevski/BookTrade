namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderDeatils_Add_CardType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "CardType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "CardType");
        }
    }
}
