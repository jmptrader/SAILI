namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntializeManagement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Managements",
                c => new
                    {
                        ManagementID = c.Int(nullable: false, identity: true),
                        LoginDateTime = c.DateTime(nullable: false),
                        UserID = c.String(maxLength: 128),
                        UrlAddress = c.String(),
                    })
                .PrimaryKey(t => t.ManagementID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Managements");
        }
    }
}
