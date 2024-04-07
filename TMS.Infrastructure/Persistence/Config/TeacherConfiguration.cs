using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
		builder.Property(t => t.Name).IsRequired().HasMaxLength(26);
		builder.Property(t => t.Email).HasMaxLength(128);
		builder.Property(t => t.Phone).IsRequired().HasMaxLength(16);
		builder.Property(t => t.JoinDate);
		builder.Property(t => t.Subject).HasConversion(
			v => v.ToString(),
			v => Enum.Parse<Subject>(v)
		);
		// Configure relationships
		builder.HasMany(t => t.Assistants)
			.WithOne()
			.HasForeignKey(a => a.TeacherId);

		builder.HasMany<Payment>()
			.WithOne()
			.HasForeignKey(p => p.TeacherId).IsRequired();

		// Configure index
		builder.HasIndex(s => s.Phone).IsUnique();
		builder.HasIndex(s => s.Email).IsUnique();
	}
}