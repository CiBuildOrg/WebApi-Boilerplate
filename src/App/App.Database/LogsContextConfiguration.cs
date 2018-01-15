using System.Data.Entity.Migrations;

namespace App.Database
{
    public class LogsContextConfiguration : DbMigrationsConfiguration<DatabaseContext>
    {
        /// <summary>
        /// this basically tells no to entity framework running migrations automatically by itself
        /// </summary>
        public LogsContextConfiguration()
        {
            // disable automatic migrations
            AutomaticMigrationsEnabled = false;
        }
    }
}
    