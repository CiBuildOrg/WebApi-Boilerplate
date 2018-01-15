using System.Data.Entity.Migrations;

namespace App.Database.LogsMigrations
{
    public class LogsContextConfiguration : DbMigrationsConfiguration<LogsDatabaseContext>
    {
        /// <summary>
        /// this basically tells no to entity framework running migrations automatically by itself
        /// </summary>
        public LogsContextConfiguration()
        {
            // disable automatic migrations
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"LogsMigrations";
        }

        protected override void Seed(LogsDatabaseContext context)
        {
            // nothing here
        }
    }
}
    