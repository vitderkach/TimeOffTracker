using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TOT.Data.Configuration;
using TOT.Entities;

namespace TOT.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        #region DbSets
        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<VacationRequest> VacationRequests { get; set; }
        public DbSet<ManagerResponse> ManagerResponses { get; set; }
        public DbSet<VacationPolicyInfo> VacationPolicies { get; set; }
        public DbSet<VacationType> VacationTypes { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new VacationRequestConfiguration());
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ManagerResponseConfiguration());
            builder.ApplyConfiguration(new UserInformationConfiguration());
            builder.ApplyConfiguration(new VacationPolicyConfiguration());
            builder.ApplyConfiguration(new VacationTypeConfiguration());
            base.OnModelCreating(builder);
        }

    }
}
