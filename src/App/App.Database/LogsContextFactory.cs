using System.Data.Entity.Infrastructure;

namespace App.Database
{
#if DEBUG

    public class LogsContextFactory : IDbContextFactory<LogsDatabaseContext>
    {
        private const string DbConnection = "Data Source=.;Initial Catalog=GenericDbLogs;Integrated Security=True;";

        LogsDatabaseContext IDbContextFactory<LogsDatabaseContext>.Create()
        {
            return new LogsDatabaseContext(DbConnection);
        }
    }

#endif

}