namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initalizeReferentialIntegrity : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Companies");
            AddColumn("dbo.StockPrices", "CompanyID", c => c.Int(nullable: false));
            AlterColumn("dbo.Companies", "CompanyID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Owners", "UserID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Owners", "PostcodeID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Companies", "CompanyID");
            CreateIndex("dbo.Owners", "UserID");
            CreateIndex("dbo.Owners", "PostcodeID");
            CreateIndex("dbo.SaltOwners", "UserID");
            CreateIndex("dbo.StockPrices", "CompanyID");
            CreateIndex("dbo.StockPrices", "SectorID");
            CreateIndex("dbo.TraderAccounts", "OwnerID");
            AddForeignKey("dbo.Owners", "PostcodeID", "dbo.Postcodes", "PostcodeID", cascadeDelete: true);
            AddForeignKey("dbo.Owners", "UserID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.SaltOwners", "UserID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.StockPrices", "CompanyID", "dbo.Companies", "CompanyID", cascadeDelete: true);
            AddForeignKey("dbo.StockPrices", "SectorID", "dbo.Sectors", "SectorID", cascadeDelete: true);
            AddForeignKey("dbo.TraderAccounts", "OwnerID", "dbo.Owners", "OwnerID", cascadeDelete: true);
            DropColumn("dbo.StockPrices", "Symbol");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockPrices", "Symbol", c => c.String());
            DropForeignKey("dbo.TraderAccounts", "OwnerID", "dbo.Owners");
            DropForeignKey("dbo.StockPrices", "SectorID", "dbo.Sectors");
            DropForeignKey("dbo.StockPrices", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.SaltOwners", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Owners", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Owners", "PostcodeID", "dbo.Postcodes");
            DropIndex("dbo.TraderAccounts", new[] { "OwnerID" });
            DropIndex("dbo.StockPrices", new[] { "SectorID" });
            DropIndex("dbo.StockPrices", new[] { "CompanyID" });
            DropIndex("dbo.SaltOwners", new[] { "UserID" });
            DropIndex("dbo.Owners", new[] { "PostcodeID" });
            DropIndex("dbo.Owners", new[] { "UserID" });
            DropPrimaryKey("dbo.Companies");
            AlterColumn("dbo.Owners", "PostcodeID", c => c.String(maxLength: 100));
            AlterColumn("dbo.Owners", "UserID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Companies", "CompanyID", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.StockPrices", "CompanyID");
            AddPrimaryKey("dbo.Companies", "CompanyID");
        }
    }
}
