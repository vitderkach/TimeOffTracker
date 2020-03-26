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

            entity.Property(mr => mr.ForStageOfApproving)
                .HasDefaultValue(1);

            entity.HasOne(r => r.VacationRequest)
                 .WithMany(mr => mr.ManagersResponses)
                 .HasForeignKey(fk => fk.VacationRequestId)
                 .HasConstraintName("FK_ManagerResponse_VacationRequest")
                 .OnDelete(DeleteBehavior.Cascade);
            /*Если удаляется заявка, удаляются и ответы к ней.
            Заявка удаляется либо самим пользователем, либо
            при удалении пользователя*/

            entity.HasOne(mr => mr.Manager)
                .WithMany(ui => ui.ManagerResponses)
                .HasForeignKey(mr => mr.ManagerId)
                .HasConstraintName("FK_ManagerResponse_UserInformation")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
