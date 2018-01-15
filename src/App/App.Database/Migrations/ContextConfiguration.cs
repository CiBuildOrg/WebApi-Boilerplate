using System.Data.Entity.Migrations;

namespace App.Database.Migrations
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
            MigrationsDirectory = @"Migrations";
            //SetSqlGenerator("System.Data.SqlClient", new MigrationScriptBuilder()); 
        }

        protected override void Seed(DatabaseContext context)
        {
            DbSeed.PopulateDatabase(context);
        }
    }
}
    