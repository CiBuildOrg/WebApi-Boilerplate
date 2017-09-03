namespace App.Database
{
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserProfileId = c.Guid(nullable: false),
                        ImageType = c.Int(nullable: false),
                        ImageSize = c.Int(nullable: false),
                        FileName = c.String(),
                        MimeType = c.String(),
                        DateStoredUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "UserProfileId", "dbo.UserProfiles");
            DropIndex("dbo.Images", new[] { "UserProfileId" });
            DropTable("dbo.Images");
        }
    }
}
