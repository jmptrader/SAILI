namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class instantateOlympicBoard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OlympicBoards",
                c => new
                    {
                        OlympicBoardID = c.Int(nullable: false, identity: true),
                        OlympicDate = c.DateTime(nullable: false),
                        sector_SectorID = c.Int(),
                    })
                .PrimaryKey(t => t.OlympicBoardID)
                .ForeignKey("dbo.Sectors", t => t.sector_SectorID)
                .Index(t => t.sector_SectorID);
            
            AddColumn("dbo.Portfolios", "OlympicBoard_OlympicBoardID", c => c.Int());
            CreateIndex("dbo.Portfolios", "OlympicBoard_OlympicBoardID");
            AddForeignKey("dbo.Portfolios", "OlympicBoard_OlympicBoardID", "dbo.OlympicBoards", "OlympicBoardID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Portfolios", "OlympicBoard_OlympicBoardID", "dbo.OlympicBoards");
            DropForeignKey("dbo.OlympicBoards", "sector_SectorID", "dbo.Sectors");
            DropIndex("dbo.OlympicBoards", new[] { "sector_SectorID" });
            DropIndex("dbo.Portfolios", new[] { "OlympicBoard_OlympicBoardID" });
            DropColumn("dbo.Portfolios", "OlympicBoard_OlympicBoardID");
            DropTable("dbo.OlympicBoards");
        }
    }
}
