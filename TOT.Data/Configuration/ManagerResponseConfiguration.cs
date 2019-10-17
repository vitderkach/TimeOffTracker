using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class ManagerResponseConfiguration : IEntityTypeConfiguration<ManagerResponse>
    {
        public void Configure(EntityTypeBuilder<ManagerResponse> builder)
        {
         /*   builder.HasKey(v => v.Id);
            builder.HasOne(p => p.VacationRequest)
                .WithMany(p => p.ManagersResponses);
    */    
    }
    }
}
