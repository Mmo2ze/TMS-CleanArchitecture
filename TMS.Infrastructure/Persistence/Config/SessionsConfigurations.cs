using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Sessions;

namespace TMS.Infrastructure.Persistence.Config;

public class SessionsConfigurations: IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);
        builder.ToTable("Sessions");
        builder.Property(s => s.Id).HasConversion(
            v => v.Value,
            v => SessionId.Create(v)
        );
        builder.Property(s => s.ClassId).IsRequired();
        builder.Property(s => s.TeacherId).IsRequired();
        builder.Property(s => s.Day).IsRequired();
        builder.Property(s => s.StartTime).IsRequired();
        builder.Property(s => s.EndTime).IsRequired();
        
        builder.HasIndex(a=> new {a.Day,a.TeacherId,a.ClassId}).IsUnique();
        
    }
}