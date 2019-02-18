namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSells : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sells",
                c => new
                {
                    SellID = c.Int(nullable: false, identity: true),
                    TransactionDate = c.DateTime(nullable: false),
                    BuyID = c.Int(nullable: false),
                    TickerSymbol = c.String(),
                    NumberOfShares = c.Int(nullable: false),
                    CostPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    SoldPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TransactionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TransactionCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PortfolioID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.SellID)
                .ForeignKey("dbo.Buys", t => t.BuyID, cascadeDelete: true)
                .Index(t => t.BuyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sells", "PortfolioID", "dbo.Portfolios");
            DropForeignKey("dbo.Sells", "BuyID", "dbo.Buys");
            DropIndex("dbo.Sells", new[] { "PortfolioID" });
            DropIndex("dbo.Sells", new[] { "BuyID" });
            DropTable("dbo.Sells");
        }
    }
}
