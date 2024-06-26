using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Models;
using TMS.Domain.RefreshTokens;
using TMS.Domain.Schedulers.Enums;
using TMS.Domain.Sessions;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Config;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasKey(t => t.Id);
        builder.ToTable("Teachers");
        // Configure properties
        builder.Property(t => t.Id).HasConversion(
            v => v.Value,
            v => TeacherId.Create(v)
        );
        builder.Property(t => t.Name).IsRequired().HasMaxLength(Constrains.Teacher.Name.Max);
        builder.Property(t => t.Email).HasMaxLength(Constrains.Email.Max);
        builder.Property(t => t.Phone).IsRequired().HasMaxLength(Constrains.Phone.Max);
        builder.Property(t => t.JoinDate);
        builder.Property(t => t.Subject).HasConversion(
            v => v.ToString(),
            v => Enum.Parse<Subject>(v)
        );
        builder.Property(t => t.Status).HasConversion(
            v => v.ToString(),
            v => Enum.Parse<TeacherStatus>(v)
        );
        builder.Property(t => t.AttendanceScheduler).HasConversion(
            v => v.ToString(),
            v => Enum.Parse<AutoAttendanceSchedulerOption>(v)
        );
        // Configure relationships
        builder.HasMany(t => t.Assistants)
            .WithOne()
            .HasForeignKey(a => a.TeacherId);
        builder.HasMany(t => t.Groups)
            .WithOne()
            .HasForeignKey(a => a.TeacherId);

      
        builder.HasMany<Session>()
            .WithOne()
            .HasForeignKey(p => p.TeacherId).IsRequired();
        builder.HasMany<RefreshToken>()
            .WithOne()
            .HasForeignKey(rt => rt.TeacherId);

        builder.HasMany(t => t.Students)
            .WithOne()
            .HasForeignKey(s => s.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure index
        builder.HasIndex(s => s.Phone).IsUnique();
        builder.HasIndex(s => s.Email).IsUnique();
    }
}