namespace App.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustRefreshTokenRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RefreshTokens", "User_Id", "dbo.ApplicationUsers");
            RenameColumn(table: "dbo.RefreshTokens", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.RefreshTokens", name: "IX_User_Id", newName: "IX_UserId");
            AddForeignKey("dbo.RefreshTokens", "UserId", "dbo.ApplicationUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefreshTokens", "UserId", "dbo.ApplicationUsers");
            RenameIndex(table: "dbo.RefreshTokens", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.RefreshTokens", name: "UserId", newName: "User_Id");
            AddForeignKey("dbo.RefreshTokens", "User_Id", "dbo.ApplicationUsers", "Id");
        }
    }
}
