namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOwner2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Owners", "PostcodeID", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Owners", "PostcodeID", c => c.String(maxLength: 7));
        }
    }
}
