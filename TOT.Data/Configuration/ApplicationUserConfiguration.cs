using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity.ToTable("ApplicationUser");
            entity.Property(p => p.Id)
                .HasColumnName("ApplicationUserID");

            entity.HasKey(p => p.Id).HasName("PK_ApplicationUser");

            entity.Property(e => e.Email)
                .HasColumnType("varchar(100)")
                .IsRequired();
            entity.HasAlternateKey(u => u.Email);

            entity.Property(em => em.NormalizedEmail)
                .HasColumnType("varchar(100)");

            entity.Property(d => d.RegistrationDate)
               .HasColumnType("datetime")
               .HasDefaultValueSql("GETDATE()");
            entity.HasIndex(r => r.RegistrationDate);

            entity.HasOne(i => i.UserInformation)
                .WithOne(u => u.User)
                .HasForeignKey<UserInformation>(a => a.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(n => n.UserName)
                .HasDefaultValue("SkipUserName")
                .IsRequired(false);

            entity.Ignore(v => v.AccessFailedCount);
            entity.Ignore(v => v.ConcurrencyStamp);
            entity.Ignore(v => v.EmailConfirmed);
            entity.Ignore(v => v.LockoutEnabled);
            entity.Ignore(v => v.LockoutEnd);
            entity.Ignore(v => v.NormalizedUserName);
            entity.Ignore(v => v.PhoneNumber);
            entity.Ignore(v => v.PhoneNumberConfirmed);
            entity.Ignore(v => v.SecurityStamp);
            entity.Ignore(v => v.TwoFactorEnabled);
            entity.Ignore(v => v.UserName);
        }
    }
}
