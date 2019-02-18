namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSector : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Postcodes", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sectors", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sectors", "SectorName", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.Sectors", "Description", c => c.String(nullable: false, maxLength: 1000));
            AddColumn("dbo.Sectors", "ImageUrl", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.Sectors", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sectors", "Name", c => c.String(nullable: false, maxLength: 30));
            DropColumn("dbo.Sectors", "ImageUrl");
            DropColumn("dbo.Sectors", "Description");
            DropColumn("dbo.Sectors", "SectorName");
            DropColumn("dbo.Sectors", "IsDeleted");
            DropColumn("dbo.Postcodes", "IsDeleted");
            DropColumn("dbo.Companies", "IsDeleted");
        }
    }
}
