namespace App.Database
{
    using System.Data.Entity.Migrations;
    
    public partial class AddRoleDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "RoleDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "RoleDescription");
        }
    }
}
