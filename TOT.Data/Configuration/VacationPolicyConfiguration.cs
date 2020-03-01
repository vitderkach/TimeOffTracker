using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class VacationPolicyConfiguration : IEntityTypeConfiguration<VacationPolicy>
    {
        public void Configure(EntityTypeBuilder<VacationPolicy> entity)
        {
            entity.HasKey(vp => new { vp.UserInformationId, vp.Year })
                .HasName("PK_VacationPolicy");

            entity.HasOne(vp => vp.UserInformation)
                .WithMany(ui => ui.VacationPolicies)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
