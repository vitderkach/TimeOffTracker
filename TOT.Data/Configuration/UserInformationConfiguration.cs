﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class UserInformationConfiguration : IEntityTypeConfiguration<UserInformation>
    {
        public void Configure(EntityTypeBuilder<UserInformation> entity)
        {
            entity.HasKey(pk => pk.UserInformationId).HasName("PK_UserInformation");

            entity.Property(n => n.FirstName)
                .HasColumnType("nvarchar(70)")
                .IsRequired();

            entity.Property(l => l.LastName)
                .HasColumnType("nvarchar(70)")
                .IsRequired();

            entity.HasIndex(fn => new { fn.FirstName, fn.LastName });

            entity.HasOne(u => u.User)
                .WithOne(i => i.UserInformation)
                .HasForeignKey<ApplicationUser>(ui => ui.UserInformationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.VacationPolicyInfo)
                .WithOne(u => u.UserInformation);

            entity.HasOne(ui => ui.Location)
                .WithMany(l => l.UserInformations)
                .HasForeignKey(ui => ui.LocationId);

            entity.HasOne(ui => ui.Team)
                .WithMany(t => t.UserInformations)
                .HasForeignKey(ui => ui.TeamId);
        }
    }
}
