namespace IT_BookTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_TradeOffers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TradeOffers",
                c => new
                    {
                        TradeOfferId = c.Int(nullable: false, identity: true),
                        UserSender = c.String(),
                        UserReceiver = c.String(),
                        Respond = c.Boolean(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                        ReceiverBook_ID = c.Int(),
                        SendersBook_ID = c.Int(),
                    })
                .PrimaryKey(t => t.TradeOfferId)
                .ForeignKey("dbo.Books", t => t.ReceiverBook_ID)
                .ForeignKey("dbo.Books", t => t.SendersBook_ID)
                .Index(t => t.ReceiverBook_ID)
                .Index(t => t.SendersBook_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TradeOffers", "SendersBook_ID", "dbo.Books");
            DropForeignKey("dbo.TradeOffers", "ReceiverBook_ID", "dbo.Books");
            DropIndex("dbo.TradeOffers", new[] { "SendersBook_ID" });
            DropIndex("dbo.TradeOffers", new[] { "ReceiverBook_ID" });
            DropTable("dbo.TradeOffers");
        }
    }
}
