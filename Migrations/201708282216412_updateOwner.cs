namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOwner : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Owners", "DOB", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Owners", "AddressNumber", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Owners", "AddressNumber", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Owners", "DOB", c => c.String(nullable: false));
        }
    }
}
