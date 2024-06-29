using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Groups;
using TMS.Domain.Sessions;

namespace TMS.Infrastructure.Persistence.Config;

public class SessionsConfigurations : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);
        builder.ToTable("Sessions");
        builder.Property(s => s.Id).HasConversion(
            v => v.Value,
            v => SessionId.Create(v)
        );
        builder.Property(s => s.GroupId).IsRequired();
        builder.Property(s => s.TeacherId).IsRequired();
        builder.Property(s => s.Day).IsRequired();
        builder.Property(s => s.StartTime).IsRequired();
        builder.Property(s => s.EndTime).IsRequired();
        builder.Property(s => s.Day).HasConversion(
            v => v.ToString(),
            v => Enum.Parse<DayOfWeek>(v)
        );
        builder.Property(s => s.Grade).HasConversion(
            v => v.ToString(),
            v => Enum.Parse<Grade>(v)
        );

        //index
        builder.HasIndex(x => new { x.TeacherId, x.Day, x.GroupId }).IsUnique();
    }
}