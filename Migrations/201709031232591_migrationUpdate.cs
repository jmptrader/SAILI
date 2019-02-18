namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrationUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Homes", "HelpPhoneNumber", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Homes", "HelpEmail", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Homes", "HelpEmail", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Homes", "HelpPhoneNumber", c => c.String(nullable: false, maxLength: 15));
        }
    }
}
