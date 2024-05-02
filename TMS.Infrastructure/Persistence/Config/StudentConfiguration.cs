using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Common.Models;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Config;

public class StudentConfiguration: IEntityTypeConfiguration<Student>
{
	public void Configure(EntityTypeBuilder<Student> builder)
	{
		builder.HasKey(s => s.Id);
		builder.ToTable("Students");
		// Configure properties
		builder.Property(s => s.Name).IsRequired();
		builder.Property(s => s.Gender).IsRequired();
		builder.Property(s => s.Email);
		builder.Property(s => s.Phone);
		builder.Property(p => p.Id)
			.HasConversion(
				v => v.Value,
				v => StudentId.Create(v)
			);
		// Configure relationships
		builder.HasMany(s => s.Payments)
			.WithOne()
			.HasForeignKey(p => p.StudentId)
			.OnDelete(DeleteBehavior.NoAction); 
		builder.HasMany(s=> s.Teachers)
			.WithMany(t => t.Students);

		builder.OwnsMany(s => s.Attendances, ab =>
		{
			ab.ToTable("Attendances");
			ab.WithOwner().HasForeignKey(a => a.StudentId);
			ab.Property(a => a.Date);
			ab.Property(a => a.Status).HasConversion(
				v => v.ToString(),
				v => Enum.Parse<AttendanceStatus>(v)
			);
			ab.HasOne<Teacher>()
				.WithMany()
				.HasForeignKey(a => a.TeacherId);
			ab.HasIndex(a => new {a.Date,a.TeacherId,a.StudentId}).IsUnique();
			builder.HasMany<RefreshToken>()
				.WithOne()
				.HasForeignKey(rt => rt.StudentId);
		});
		
		// Configure indexes
		builder.HasIndex(s => s.Phone).IsUnique();
		builder.HasIndex(s => s.Email).IsUnique();
		
		

	}
}