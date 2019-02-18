namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeCompany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        Symbol = c.String(nullable: false, maxLength: 10),
                        CompanyName = c.String(nullable: false, maxLength: 50),
                        SectorID = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyID);
            
            CreateIndex("dbo.StockPrices", "CompanyID");
            AddForeignKey("dbo.StockPrices", "CompanyID", "dbo.Companies", "CompanyID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockPrices", "CompanyID", "dbo.Companies");
            DropIndex("dbo.StockPrices", new[] { "CompanyID" });
            DropTable("dbo.Companies");
        }
    }
}
