using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    public class VacationRequestConfiguration: IEntityTypeConfiguration<VacationRequest>
    {
        public void Configure(EntityTypeBuilder<VacationRequest> builder)
        {

            /* builder.HasKey(v => v.VacationRequestId);
             builder.HasMany(p => p.ManagersResponses)
                 .WithOne(p => p.VacationRequest);
             builder.HasOne(p => p.User)
                 .WithMany(p => p.VacationRequests);
                 */

           builder.HasOne(p => p.User)
          .WithMany(p => p.VacationRequests)
          .IsRequired()
          .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
