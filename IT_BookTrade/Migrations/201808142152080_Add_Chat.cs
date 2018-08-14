namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Chat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        ChatId = c.Int(nullable: false, identity: true),
                        User1 = c.String(),
                        User2 = c.String(),
                    })
                .PrimaryKey(t => t.ChatId);
            
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        ChatMessagesId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Date = c.DateTime(nullable: false),
                        PostedBy = c.Boolean(nullable: false),
                        ChatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChatMessagesId)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .Index(t => t.ChatId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "ChatId", "dbo.Chats");
            DropIndex("dbo.ChatMessages", new[] { "ChatId" });
            DropTable("dbo.ChatMessages");
            DropTable("dbo.Chats");
        }
    }
}
