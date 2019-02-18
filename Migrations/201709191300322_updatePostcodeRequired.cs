namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePostcodeRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Owners", "PostcodeID", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Owners", "PostcodeID", c => c.String());
        }
    }
}
