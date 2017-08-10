namespace App.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkRefreshTokenToClients : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RefreshTokens", "ClientId", c => c.Guid(nullable: false));
            CreateIndex("dbo.RefreshTokens", "ClientId");
            AddForeignKey("dbo.RefreshTokens", "ClientId", "dbo.Clients", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefreshTokens", "ClientId", "dbo.Clients");
            DropIndex("dbo.RefreshTokens", new[] { "ClientId" });
            AlterColumn("dbo.RefreshTokens", "ClientId", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
