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
            entity.HasKey(vt => new { vt.VacationPolicyId, vt.TimeOffType })
                .HasName("PK_ VacationType");

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

            entity.HasOne(vt => vt.VacationPolicy)
                .WithMany(vp => vp.VacationTypes)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
