using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public DbSet<VacationType> VacationTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Team> Teams { get; set; }
        #endregion

        #region DbQueries
        public DbQuery<ManagerResponseHistory> ManagerResponseHistories { get; set; }
        public DbQuery<VacationRequestHistory> VacationRequestHistories { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Query<VacationRequestHistory>().Property(vrh => vrh.VacationType).HasConversion(new EnumToStringConverter<TimeOffType>());
            builder.ApplyConfiguration(new VacationRequestConfiguration());
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new LocationConfiguration());
            builder.ApplyConfiguration(new TeamConfiguration());
            builder.ApplyConfiguration(new ManagerResponseConfiguration());
            builder.ApplyConfiguration(new UserInformationConfiguration());
            builder.ApplyConfiguration(new VacationTypeConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
