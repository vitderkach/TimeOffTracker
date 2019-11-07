using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Data.Configuration {
    public class VacationTypeConfiguration : IEntityTypeConfiguration<VacationType> {
        public void Configure(EntityTypeBuilder<VacationType> entity)
        {
            entity.HasKey(r => r.Id);

            var converter = new EnumToStringConverter<TimeOffType>();

            entity.Property(t => t.TimeOffType)
                .HasColumnType("nvarchar(30)")
                .HasConversion(converter)
                .IsRequired();
        }
    }
}
