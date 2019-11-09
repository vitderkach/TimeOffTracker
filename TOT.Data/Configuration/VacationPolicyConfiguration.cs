using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Data.Configuration {
    class VacationPolicyConfiguration: IEntityTypeConfiguration<VacationPolicyInfo> {
        public void Configure(EntityTypeBuilder<VacationPolicyInfo> entity)
        {
            entity.HasKey(r => r.Id);

            entity.HasOne(v => v.UserInformation)
                .WithOne(vi => vi.VacationPolicyInfo)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
