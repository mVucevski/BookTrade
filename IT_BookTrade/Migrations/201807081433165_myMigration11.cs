namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class myMigration11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Rating", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "Rating", c => c.Int(nullable: false));
        }
    }
}
