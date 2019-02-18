namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateManagementLogOff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Managements", "LogOffDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Managements", "LogOffDateTime");
        }
    }
}
