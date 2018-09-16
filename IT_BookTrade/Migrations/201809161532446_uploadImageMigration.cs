namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uploadImageMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "ImagePath", c => c.String());
            DropColumn("dbo.Books", "ImageURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "ImageURL", c => c.String(nullable: false));
            DropColumn("dbo.Books", "ImagePath");
        }
    }
}
