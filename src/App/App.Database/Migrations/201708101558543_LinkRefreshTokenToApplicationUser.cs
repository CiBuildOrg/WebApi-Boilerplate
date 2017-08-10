namespace App.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkRefreshTokenToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RefreshTokens", "User_Id", c => c.Guid(nullable: false));
            CreateIndex("dbo.RefreshTokens", "User_Id");
            AddForeignKey("dbo.RefreshTokens", "User_Id", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefreshTokens", "User_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.RefreshTokens", new[] { "User_Id" });
            DropColumn("dbo.RefreshTokens", "User_Id");
        }
    }
}
