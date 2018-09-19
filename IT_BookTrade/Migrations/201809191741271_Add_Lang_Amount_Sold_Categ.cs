namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Lang_Amount_Sold_Categ : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Language", c => c.String(nullable: false));
            AddColumn("dbo.Books", "Amount", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "BooksSold", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "Category", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "Category");
            DropColumn("dbo.Books", "BooksSold");
            DropColumn("dbo.Books", "Amount");
            DropColumn("dbo.Books", "Language");
        }
    }
}
