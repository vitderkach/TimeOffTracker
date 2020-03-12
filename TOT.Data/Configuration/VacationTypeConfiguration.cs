using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    public class VacationTypeConfiguration : IEntityTypeConfiguration<VacationType>
    {
        public void Configure(EntityTypeBuilder<VacationType> entity)
        {
            entity.HasKey(vt => vt.Id)
                .HasName("PK_ VacationType");

            entity.HasAlternateKey(vt => new { vt.UserInformationId, vt.Year, vt.TimeOffType })
                .HasName("PK_ VacationType_UserInformationId_Year_TimeOffType");

            var converter = new EnumToStringConverter<TimeOffType>();

            entity.Property(vt => vt.TimeOffType)
                .HasColumnType("nvarchar(30)")
                .HasColumnName("Name")
                .HasConversion(converter);

            entity.Property(vt => vt.StatutoryDays)
                .IsRequired();

            entity.Property(vt => vt.UsedDays)
                .IsRequired()
                .HasDefaultValue(0);

            entity.HasOne(vt => vt.UserInformation)
                .WithMany(ui => ui.VacationTypes)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
