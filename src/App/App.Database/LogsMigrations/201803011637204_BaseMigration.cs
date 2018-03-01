namespace App.Database.LogsMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Traces",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CallerIdentity = c.String(nullable: false, maxLength: 256),
                        RequestTimestamp = c.DateTime(nullable: false),
                        Verb = c.String(nullable: false, maxLength: 16),
                        Url = c.String(nullable: false, maxLength: 128),
                        RequestPayload = c.String(storeType: "ntext"),
                        StatusCode = c.Int(nullable: false),
                        ReasonPhrase = c.String(nullable: false, maxLength: 64),
                        RequestHeaders = c.String(maxLength: 4000),
                        ResponseHeaders = c.String(maxLength: 4000),
                        ResponsePayload = c.String(storeType: "ntext"),
                        CallDuration = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Traces");
        }
    }
}
