namespace IT_BookTrade_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class myMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "BookAuthor", c => c.String(nullable: false));
            AddColumn("dbo.Books", "BookDescription", c => c.String());
            AddColumn("dbo.Books", "Description", c => c.String());
            AddColumn("dbo.Books", "Price", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "Tradeable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "Tradeable");
            DropColumn("dbo.Books", "Price");
            DropColumn("dbo.Books", "Description");
            DropColumn("dbo.Books", "BookDescription");
            DropColumn("dbo.Books", "BookAuthor");
        }
    }
}
