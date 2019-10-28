using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity.Property(p => p.Id)
                .HasColumnName("ApplicationUserId");

            entity.Property(e => e.Email)
                .HasColumnType("varchar(100)")
                .IsRequired();
            entity.HasIndex(em => em.Email).IsUnique();

            entity.Property(em => em.NormalizedEmail)
                .HasColumnType("varchar(100)");

            entity.Property(n => n.UserName)
                .HasColumnType("nvarchar(50)");

            entity.Property(no => no.NormalizedUserName)
                .HasColumnType("nvarchar(50)")
                .IsRequired(false);

            entity.Property(d => d.RegistrationDate)
               .HasColumnType("datetime")
               .HasDefaultValueSql("GETUTCDATE()");
               
            entity.Ignore(v => v.TwoFactorEnabled);
        }
    }
}
