using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Data.Configuration;
using TOT.Entities;

namespace TOT.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
        }
        #region DbSets
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<VacationRequest> VacationRequests { get; set; }
        public DbSet<ManagerResponse> ManagerResponses { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new VacationRequestConfiguration());
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ManagerResponseConfiguration());
            builder.ApplyConfiguration(new UserInformationConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
