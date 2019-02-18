namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeTraderAccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TraderAccounts",
                c => new
                    {
                        TradingAccountID = c.String(nullable: false, maxLength: 128),
                        OwnerID = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TradingAccountID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TraderAccounts");
        }
    }
}
