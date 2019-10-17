using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
           /* builder.HasKey(v => v.Id);
            builder.HasMany(p => p.VacationRequests)
                .WithOne(p => p.User)
                .HasForeignKey(p=>p.ApplicationUserId);
            builder.HasOne(p => p.UserInformation)
                .WithOne(p => p.User);
                */
        }
    }
}
