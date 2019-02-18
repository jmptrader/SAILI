namespace SAILI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initializeSaltOwner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaltOwners",
                c => new
                    {
                        EncryptOwnerID = c.Int(nullable: false, identity: true),
                        SaltDOB = c.String(maxLength: 128),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EncryptOwnerID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SaltOwners");
        }
    }
}
