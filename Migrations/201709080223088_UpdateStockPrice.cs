namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStockPrice : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StockPrices", "SectorID", "dbo.Sectors");
            DropIndex("dbo.StockPrices", new[] { "SectorID" });
            AddColumn("dbo.Companies", "SectorID", c => c.Int(nullable: false));
            DropColumn("dbo.StockPrices", "SectorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockPrices", "SectorID", c => c.Int(nullable: false));
            DropColumn("dbo.Companies", "SectorID");
            CreateIndex("dbo.StockPrices", "SectorID");
            AddForeignKey("dbo.StockPrices", "SectorID", "dbo.Sectors", "SectorID", cascadeDelete: true);
        }
    }
}
