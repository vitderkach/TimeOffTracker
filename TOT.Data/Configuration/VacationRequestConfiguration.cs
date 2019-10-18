using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    public class VacationRequestConfiguration: IEntityTypeConfiguration<VacationRequest>
    {
        public void Configure(EntityTypeBuilder<VacationRequest> entity)
        {
            entity.HasKey(r => r.VacationRequestId)
                .HasName("PK_VacationRequest");

            entity.Property(s => s.StartDate)
                .HasColumnType("date")
                .IsRequired();
            entity.HasIndex(d => d.StartDate);

            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .IsRequired();
            entity.HasIndex(d => d.EndDate);

            entity.Property(n => n.Notes)
                .HasColumnType("nvarchar(200)");

            entity.Property(c => c.CreationDate)
               .HasColumnType("datetime")
               .HasDefaultValueSql("GETDATE()");
            entity.HasIndex(cr => cr.CreationDate);

            var converter = new EnumToStringConverter<TimeOffType>();
            entity.Property(t => t.VacationType)
                .HasColumnType("nvarchar(30)")
                .HasConversion(converter)
                .IsRequired();
            entity.HasIndex(t => t.VacationType);

            entity.HasOne(u => u.User)
                .WithMany(r => r.VacationRequests)
                .HasForeignKey(fk => fk.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
