using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Attendances;

namespace TMS.Infrastructure.Persistence.Config;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("Attendances");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(
            v => v.Value,
            v => new AttendanceId(v));
        builder.Property(x => x.Status)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<AttendanceStatus>(v)
            );

        builder.Property(x => x.CreatedById).IsRequired(false);
        builder.Property(x => x.UpdatedById).IsRequired(false);
        builder.Property(x => x.UpdatedAt).IsRequired(false);


        // Relationships
        builder.HasOne(x => x.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId);


        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .HasForeignKey(x => x.CreatedById);

        builder.HasOne(x => x.UpdatedBy)
            .WithMany()
            .HasForeignKey(x => x.UpdatedById);

        builder.HasOne(x => x.Account)
            .WithMany(x => x.Attendances)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => new { x.AccountId, x.Date }).IsUnique();
    }
}