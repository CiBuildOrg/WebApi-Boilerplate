namespace App.Database
{
    using System.Data.Entity.Migrations;
    
    public partial class RenameAspTables : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AspNetRoles", newName: "ApplicationRoles");
            RenameTable(name: "dbo.AspNetUserRoles", newName: "ApplicationUserRoles");
            RenameTable(name: "dbo.AspNetUsers", newName: "ApplicationUsers");
            RenameTable(name: "dbo.AspNetUserClaims", newName: "ApplicationUserClaims");
            RenameTable(name: "dbo.AspNetUserLogins", newName: "ApplicationUserLogins");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ApplicationUserLogins", newName: "AspNetUserLogins");
            RenameTable(name: "dbo.ApplicationUserClaims", newName: "AspNetUserClaims");
            RenameTable(name: "dbo.ApplicationUsers", newName: "AspNetUsers");
            RenameTable(name: "dbo.ApplicationUserRoles", newName: "AspNetUserRoles");
            RenameTable(name: "dbo.ApplicationRoles", newName: "AspNetRoles");
        }
    }
}
