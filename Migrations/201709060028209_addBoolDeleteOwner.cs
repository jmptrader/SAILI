namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBoolDeleteOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Owners", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Owners", "IsDeleted");
        }
    }
}
