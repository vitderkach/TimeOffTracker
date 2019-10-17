using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class UserInformationConfiguration : IEntityTypeConfiguration<UserInformation>
    {
        public void Configure(EntityTypeBuilder<UserInformation> builder)
        {
           /* builder.HasKey(v => v.UserInformationId);
            builder.HasOne(o => o.User)
                .WithOne(p => p.UserInformation);
    */    
    }
    }
}
