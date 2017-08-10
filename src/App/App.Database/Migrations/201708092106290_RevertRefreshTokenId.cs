namespace App.Database
{
    using System.Data.Entity.Migrations;
    
    public partial class RevertRefreshTokenId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.RefreshTokens");
            AlterColumn("dbo.RefreshTokens", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.RefreshTokens", "Id");
            DropColumn("dbo.RefreshTokens", "RefreshTokenId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RefreshTokens", "RefreshTokenId", c => c.String());
            DropPrimaryKey("dbo.RefreshTokens");
            AlterColumn("dbo.RefreshTokens", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.RefreshTokens", "Id");
        }
    }
}
