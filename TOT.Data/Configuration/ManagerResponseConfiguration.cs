using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TOT.Entities;

namespace TOT.Data.Configuration
{
    class ManagerResponseConfiguration : IEntityTypeConfiguration<ManagerResponse>
    {
        public void Configure(EntityTypeBuilder<ManagerResponse> entity)
        {
            entity.HasKey(r => r.Id)
                .HasName("PK_ManagerResponse");

            entity.Property(d => d.DateResponse)
               .HasColumnType("datetime")
               .HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(d => d.DateResponse);

            entity.Property(n => n.Notes)
                .HasColumnType("nvarchar(200)");

            /*entity.HasOne(m => m.Manager)
                .WithMany(mr => mr.ManagerResponses)
                .HasForeignKey(fk => fk.ManagerId)
                .HasConstraintName("FK_Manager_Responses")
                .OnDelete(DeleteBehavior.Restrict); 
            */
            entity.HasOne(r => r.VacationRequest)
                 .WithMany(mr => mr.ManagersResponses)
                 .HasForeignKey(fk => fk.VacationRequestId)
                 .HasConstraintName("FK_Request_Responses")
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
