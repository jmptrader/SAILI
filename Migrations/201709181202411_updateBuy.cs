namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateBuy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Portfolios", "TradingAccountID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Portfolios", "TradingAccountID");
            AddForeignKey("dbo.Portfolios", "TradingAccountID", "dbo.TraderAccounts", "TradingAccountID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Portfolios", "TradingAccountID", "dbo.TraderAccounts");
            DropIndex("dbo.Portfolios", new[] { "TradingAccountID" });
            DropColumn("dbo.Portfolios", "TradingAccountID");
        }
    }
}
