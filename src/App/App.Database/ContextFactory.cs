using System.Data.Entity.Infrastructure;

namespace App.Database
{
#if DEBUG

    public class ContextFactory : IDbContextFactory<DatabaseContext>
    {
        private const string DbConnection = "Data Source=.;Initial Catalog=GenericDb;Integrated Security=True;";

        DatabaseContext IDbContextFactory<DatabaseContext>.Create()
        {
            return new DatabaseContext(DbConnection);
        }
    }

#endif

}