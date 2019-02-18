namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePostcodeId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Owners", "PostcodeID", "dbo.Postcodes");
            DropIndex("dbo.Owners", new[] { "PostcodeID" });
            AlterColumn("dbo.Owners", "PostcodeID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Owners", "PostcodeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Owners", "PostcodeID");
            AddForeignKey("dbo.Owners", "PostcodeID", "dbo.Postcodes", "PostcodeID", cascadeDelete: true);
        }
    }
}
