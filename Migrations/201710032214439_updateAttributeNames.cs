namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAttributeNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Buys", "TradeDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Buys", "PurchasePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Buys", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Sells", "TradeDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sells", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Sells", "PurchasePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Buys", "TransactionDate");
            DropColumn("dbo.Buys", "NumberOfShares");
            DropColumn("dbo.Buys", "CostPerShare");
            DropColumn("dbo.Sells", "TransactionDate");
            DropColumn("dbo.Sells", "NumberOfShares");
            DropColumn("dbo.Sells", "CostPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sells", "CostPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Sells", "NumberOfShares", c => c.Int(nullable: false));
            AddColumn("dbo.Sells", "TransactionDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Buys", "CostPerShare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Buys", "NumberOfShares", c => c.Int(nullable: false));
            AddColumn("dbo.Buys", "TransactionDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Sells", "PurchasePrice");
            DropColumn("dbo.Sells", "Quantity");
            DropColumn("dbo.Sells", "TradeDate");
            DropColumn("dbo.Buys", "Quantity");
            DropColumn("dbo.Buys", "PurchasePrice");
            DropColumn("dbo.Buys", "TradeDate");
        }
    }
}
