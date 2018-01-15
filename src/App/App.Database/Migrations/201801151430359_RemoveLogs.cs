namespace App.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLogs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LogSteps", "LogEntryId", "dbo.LogEntries");
            DropIndex("dbo.LogSteps", new[] { "LogEntryId" });
            DropTable("dbo.LogEntries");
            DropTable("dbo.LogSteps");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LogSteps",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Index = c.Int(nullable: false),
                        StepTimestamp = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Source = c.String(),
                        Name = c.String(),
                        Frame = c.String(),
                        Metadata = c.String(),
                        Message = c.String(),
                        LogEntryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        RequestTimestamp = c.DateTime(),
                        ResponseTimestamp = c.DateTime(),
                        RequestUri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.LogSteps", "LogEntryId");
            AddForeignKey("dbo.LogSteps", "LogEntryId", "dbo.LogEntries", "Id", cascadeDelete: true);
        }
    }
}
