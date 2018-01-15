using App.Database.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.SqlServer;
using System.Text.RegularExpressions;

namespace App.Database
{
    public class MigrationScriptBuilder : SqlServerMigrationSqlGenerator
    {
        private readonly string _contextName;
        private readonly Regex _matchingRegex = new Regex(@"\(N'(.*)', N", RegexOptions.Compiled);

        public MigrationScriptBuilder()
        {
            _contextName = $"N'{typeof(ContextConfiguration).FullName}'";
        }

        protected override void Generate(SqlOperation sqlOperation)
        {
            Statement("GO");
            base.Generate(sqlOperation);
            Statement("GO");
        }

        public override IEnumerable<MigrationStatement> Generate(IEnumerable<MigrationOperation> migrationOperations, string providerManifestToken)
        {
            var transactionName = string.Empty;// UniqueName();

            var migrationStatements = base.Generate(migrationOperations, providerManifestToken);

            yield return new MigrationStatement { Sql = "BEGIN TRY" };
            yield return new MigrationStatement { Sql = $"BEGIN TRANSACTION {transactionName}" };

            var migrationNameFound = false;
            var migrationName = string.Empty;

            foreach (var migrationStatement in migrationStatements)
            {

                var processMigrationResult = ProcessMigration(migrationStatement);
                if (!migrationNameFound && processMigrationResult.Success)
                {
                    // we have the migration name 
                    migrationNameFound = true;
                    migrationName = processMigrationResult.MigrationName;
                }

                yield return migrationStatement;
            }

            yield return new MigrationStatement { Sql = $"COMMIT TRANSACTION {transactionName}" };
            yield return new MigrationStatement { Sql = "END TRY " };
            yield return new MigrationStatement { Sql = "BEGIN CATCH " };

            if (migrationNameFound)
            {
                yield return new MigrationStatement
                {
                    Sql = $"RAISERROR('Error raised in migration {migrationName}', 16, 1);"
                };
            }
            else
            {
                yield return new MigrationStatement
                {
                    Sql = $"RAISERROR('Error raised while executing migration', 16, 1);"
                };
            }

            yield return new MigrationStatement { Sql = $"ROLLBACK TRANSACTION {transactionName}" };
            yield return new MigrationStatement { Sql = "END CATCH " };
        }

        private static string UniqueName() => Guid.NewGuid().ToString("N");

        private class MigrationMatch
        {
            public bool Success { get; set; }
            public string MigrationName { get; set; }

            public static MigrationMatch Miss => new MigrationMatch { Success = false };
        }

        private MigrationMatch ProcessMigration(MigrationStatement statement)
        {
            var sql = statement.Sql;

            if (!sql.Contains(_contextName)) return MigrationMatch.Miss;

            // we are on the line where the migration is declared
            var migrationNameMatch = _matchingRegex.Match(sql);

            if (!migrationNameMatch.Success)
                return MigrationMatch.Miss;

            var firstMatch = migrationNameMatch.Groups[0].ToString();
            var migrationName = firstMatch.Substring(3, firstMatch.Length - 7);

            return new MigrationMatch
            {
                Success = true,
                MigrationName = migrationName
            };
        }
    }
}
