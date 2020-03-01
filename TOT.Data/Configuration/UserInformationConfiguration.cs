using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class UserInformationConfiguration : IEntityTypeConfiguration<UserInformation>
    {
        public void Configure(EntityTypeBuilder<UserInformation> entity)
        {
            entity.HasKey(ui => ui.ApplicationUserId)
                .HasName("PK_UserInformation");

            entity.Property(ui => ui.FirstName)
                .HasColumnType("nvarchar(70)")
                .IsRequired();

            entity.Property(ui => ui.LastName)
                .HasColumnType("nvarchar(70)")
                .IsRequired();

            entity.HasIndex(ui => new { ui.FirstName, ui.LastName });

            entity.Property(ui => ui.IsFired)
                .IsRequired();

            entity.Property(ui => ui.RecruitmentDate)
                .HasColumnType("date")
                .HasDefaultValue(DateTime.Now.Date);

            entity.HasOne(ui => ui.ApplicationUser)
                .WithOne(au => au.UserInformation)
                .HasForeignKey<ApplicationUser>(au => au.UserInformationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ui => ui.Location)
                .WithMany(l => l.UserInformations);

            entity.HasOne(ui => ui.Team)
                .WithMany(t => t.UserInformations);
        }
    }
}
