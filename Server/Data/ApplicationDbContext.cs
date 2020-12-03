using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TewiClipool.Server.Models;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<AppUser>
    {
        private readonly IConfiguration _configuration;

        public DbSet<RegisterToken> RegisterTokens { get; set; }
        public DbSet<AnnounceInfo> AnnounceInfos { get; set; }
        public DbSet<Clip> Clips { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<StaffItem> StaffItems { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IConfiguration configuration) : base(options, operationalStoreOptions)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Rename Tables
            builder.Entity<AppUser>().ToTable(Literal.DbTables.Users);

            builder.Entity<IdentityUserClaim<string>>().ToTable(Literal.DbTables.UserClaims);
            builder.Entity<IdentityUserLogin<string>>().ToTable(Literal.DbTables.UserLogins);
            builder.Entity<IdentityUserToken<string>>().ToTable(Literal.DbTables.UserTokens);

            builder.Entity<IdentityRole>().ToTable(Literal.DbTables.UserRoles);
            builder.Entity<IdentityRoleClaim<string>>().ToTable(Literal.DbTables.UserRoleClaims);
            builder.Entity<IdentityUserRole<string>>().ToTable(Literal.DbTables.UserRole);

            // Seed Data
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = Literal.Roles.Admin,
                    NormalizedName = Literal.Roles.Admin.ToUpper()
                },
                new IdentityRole
                {
                    Name = Literal.Roles.Operator,
                    NormalizedName = Literal.Roles.Operator.ToUpper()
                },
                new IdentityRole
                {
                    Name = Literal.Roles.User,
                    NormalizedName = Literal.Roles.User.ToUpper()
                },
                new IdentityRole
                {
                    Name = Literal.Roles.Editor,
                    NormalizedName = Literal.Roles.Editor.ToUpper()
                },
                new IdentityRole
                {
                    Name = Literal.Roles.Translator,
                    NormalizedName = Literal.Roles.Translator.ToUpper()
                },
                new IdentityRole
                {
                    Name = Literal.Roles.Timeline,
                    NormalizedName = Literal.Roles.Timeline.ToUpper()
                },
                new IdentityRole
                {
                    Name = Literal.Roles.Reviewer,
                    NormalizedName = Literal.Roles.Reviewer.ToUpper()
                },
                new IdentityRole
                {
                    Name = Literal.Roles.PostProducer,
                    NormalizedName = Literal.Roles.PostProducer.ToUpper()
                },
                new IdentityRole
                {
                    Name = Literal.Roles.Art,
                    NormalizedName = Literal.Roles.Art.ToUpper()
                });

            builder.Entity<RegisterToken>().HasData(
                new RegisterToken
                {
                    Id = -1,
                    Token = _configuration.GetValue<string>(Literal.AdminRegisterToken)
                });
        }
    }
}
