using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Groups;
using TMS.Domain.Schedulers;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Config;

public class SchedulerConfiguration : IEntityTypeConfiguration<Scheduler>
{
    public void Configure(EntityTypeBuilder<Scheduler> builder)
    {
        builder.ToTable("Schedulers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x =>
                    x.Value,
                x => new SchedulerId(x));
        builder.Property(x => x.Grade).HasConversion(x =>
                x.ToString(),
            x => !string.IsNullOrEmpty(x) ? Enum.Parse<Grade>(x) : null);
        builder.Property(x => x.Day).HasConversion(x =>
                x.ToString(),
            x => Enum.Parse<DayOfWeek>(x));
        

        // relationships
        builder.HasOne<Teacher>()
            .WithMany(x => x.AttendanceSchedulers)
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        // indexes
        builder.HasIndex(x =>
                new { x.TeacherId, x.Day, x.FiresOn })
            .IsUnique();
        builder.HasIndex(x =>
                new { x.TeacherId, x.Day, x.Grade })
            .IsUnique();
    }
}