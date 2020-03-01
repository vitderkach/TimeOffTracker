using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class VacationDayConfiguration : IEntityTypeConfiguration<VacationDay>
    {
        public void Configure(EntityTypeBuilder<VacationDay> entity)
        {
            entity.HasKey(vd => vd.Id)
                .HasName("PK_VacationDay");

            entity.Property(vd => vd.DefaultVacationCount)
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(vd => vd.GiftVacationCount)
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}
