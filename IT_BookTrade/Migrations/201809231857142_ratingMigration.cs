namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratingMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "RatingNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "RatingSum", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "RatingSum");
            DropColumn("dbo.Books", "RatingNumber");
        }
    }
}
