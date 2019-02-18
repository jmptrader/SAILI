namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalizeShareTrading : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyID = c.String(nullable: false, maxLength: 128),
                        CompanyName = c.String(),
                    })
                .PrimaryKey(t => t.CompanyID);
            
            CreateTable(
                "dbo.Sectors",
                c => new
                    {
                        SectorID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.SectorID);
            
            CreateTable(
                "dbo.StockPrices",
                c => new
                    {
                        SharePriceID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Symbol = c.String(),
                        Open = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Closing = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PercentageChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PE = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EPS = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SectorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SharePriceID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StockPrices");
            DropTable("dbo.Sectors");
            DropTable("dbo.Companies");
        }
    }
}
