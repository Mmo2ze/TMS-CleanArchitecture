using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.AttendanceSchedulers;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Config;

public class AttendanceSchedulerConfiguration : IEntityTypeConfiguration<AttendanceScheduler>
{
    public void Configure(EntityTypeBuilder<AttendanceScheduler> builder)
    {
        builder.ToTable("AttendanceSchedulers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x =>
                    x.Value,
                x => new AttendanceSchedulerId(x));
        builder.Property(x => x.Grade).IsRequired(false);

        // relationships
        builder.HasOne<Teacher>()
            .WithMany(x=> x.AttendanceSchedulers)
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        // indexes
        builder.HasIndex(x =>
                new { x.TeacherId, x.Day, x.StartTime })
            .IsUnique();
    }
}