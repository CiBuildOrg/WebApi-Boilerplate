using System.Data.Entity.Migrations;

namespace App.Database
{
    public class ContextConfiguration : DbMigrationsConfiguration<DatabaseContext>
    {
        /// <summary>
        /// this basically tells no to entity framework running migrations automatically by itself
        /// </summary>
        public ContextConfiguration()
        {
            // disable automatic migrations
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DatabaseContext context)
        {
            DbSeed.PopulateDatabase(context);
        }
    }
}
    