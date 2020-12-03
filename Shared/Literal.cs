namespace TewiClipool.Shared
{
    public static class Literal
    {
        public static class ApiUrls
        {
            public const string Announce = "api/announce";
            public const string Clip = "api/clips";
            public const string Comment = "api/comments";
            public const string RegisterToken = "api/registertokens";
            public const string Staff = "api/staff";
            public const string User = "api/users";
        }

        public static class DbTables
        {
            public const string Users = "Users";
            public const string UserClaims = "UserClaims";
            public const string UserLogins = "UserLogins";
            public const string UserTokens = "UserTokens";
            public const string UserRoles = "UserRoles";
            public const string UserRoleClaims = "UserRoleClaims";
            public const string UserRole = "UserRole";
        }

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Operator = "Operator";
            public const string User = "User";

            public const string Editor = "Editor";
            public const string Translator = "Translator";
            public const string Timeline = "Timeline";
            public const string Reviewer = "Reviewer";
            public const string PostProducer = "PostProducer";
            public const string Art = "Art";
        }

        public const string DatabasePath = "DatabasePath";
        public const string AdminRegisterToken = "AdminRegisterToken";
        public const string Domain = "Domain";
    }
}
