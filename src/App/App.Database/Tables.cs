namespace App.Database
{
    internal class Tables
    {
        internal const string Schema = "dbo";
        internal const string DummyTable = "Dummy";
        internal const string TracesTable = "Traces";
        internal const string UserProfileTable = "UserProfiles";
        internal const string ImageTable = "Images";
        internal const string ClientsTable = "Clients";
        internal static string RefreshTokensTable = "RefreshTokens";

        // asp identity tables
        internal static string ApplicationUsersTable = "ApplicationUsers";

        internal static string ApplicationIdentityUserClaimsTable = "ApplicationUserClaims";

        internal static string ApplicationIdentityUserLoginsTable = "ApplicationUserLogins";

        internal static string ApplicationIdentityUserRolesTable = "ApplicationUserRoles";

        internal static string ApplicationRolesTable = "ApplicationRoles";
    }
}
