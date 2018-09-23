namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratingMigration2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Ratings", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "RatingsSum", c => c.Int(nullable: false));
            DropColumn("dbo.Books", "Rating");
            DropColumn("dbo.Books", "RatingNumber");
            DropColumn("dbo.Books", "RatingSum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "RatingSum", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "RatingNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "Rating", c => c.Double(nullable: false));
            DropColumn("dbo.Books", "RatingsSum");
            DropColumn("dbo.Books", "Ratings");
        }
    }
}
