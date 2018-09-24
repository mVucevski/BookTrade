namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Who_Rated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rateds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User = c.String(),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Rateds");
        }
    }
}
