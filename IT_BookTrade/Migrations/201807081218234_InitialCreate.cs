namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        BookAuthor = c.String(nullable: false),
                        BookDescription = c.String(),
                        Description = c.String(),
                        ImageURL = c.String(nullable: false),
                        Rating = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Tradeable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Books");
        }
    }
}
