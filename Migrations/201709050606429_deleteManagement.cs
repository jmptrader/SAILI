namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteManagement : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Managements");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Managements",
                c => new
                    {
                        ManagementID = c.Int(nullable: false, identity: true),
                        LoginDateTime = c.DateTime(nullable: false),
                        LogOffDateTime = c.DateTime(nullable: false),
                        UserID = c.String(maxLength: 128),
                        UrlAddress = c.String(),
                    })
                .PrimaryKey(t => t.ManagementID);
            
        }
    }
}
