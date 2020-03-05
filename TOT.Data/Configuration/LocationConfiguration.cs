using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TOT.Entities;
namespace TOT.Data.Configuration
{
    class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> entity)
        {
            entity.HasKey(l => l.Id)
                .HasName("PK_Location");

            entity.Property(l => l.Name)
                .HasColumnType("nvarchar(60)")
                .IsRequired();

            entity.HasAlternateKey(l => l.Name);
        }
    }
}
