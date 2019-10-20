﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class UserInformationConfiguration : IEntityTypeConfiguration<UserInformation>
    {
        public void Configure(EntityTypeBuilder<UserInformation> entity)
        {
            entity.HasKey(pk => pk.Id).HasName("PK_UserInformation");

            entity.Property(n => n.FirstName)
                .HasColumnType("nvarchar(70)")
                .IsRequired();

            entity.Property(l => l.LastName)
                .HasColumnType("nvarchar(70)")
                .IsRequired();

            entity.HasIndex(fn => new { fn.FirstName, fn.LastName });
        }
    }
}
