using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> entity)
        {
            entity.HasKey(t => t.Id)
                .HasName("PK_Team");

            entity.Property(t => t.Name)
                .HasColumnType("nvarchar(60)")
                .IsRequired();

            entity.HasAlternateKey(t => t.Name);
        }
    }
}
