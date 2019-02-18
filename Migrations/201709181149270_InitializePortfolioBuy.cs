namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializePortfolioBuy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Buys",
                c => new
                    {
                        BuyID = c.Int(nullable: false, identity: true),
                        TransactionDate = c.DateTime(nullable: false),
                        PortfolioId = c.Int(nullable: false),
                        TickerSymbol = c.String(),
                        NumberOfShares = c.Int(nullable: false),
                        CostPerShare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.BuyID)
                .ForeignKey("dbo.Portfolios", t => t.PortfolioId, cascadeDelete: true)
                .Index(t => t.PortfolioId);
            
            CreateTable(
                "dbo.Portfolios",
                c => new
                    {
                        PortfolioID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.PortfolioID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Buys", "PortfolioId", "dbo.Portfolios");
            DropIndex("dbo.Buys", new[] { "PortfolioId" });
            DropTable("dbo.Portfolios");
            DropTable("dbo.Buys");
        }
    }
}
