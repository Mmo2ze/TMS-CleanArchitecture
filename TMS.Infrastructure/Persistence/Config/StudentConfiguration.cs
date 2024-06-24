using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Common.Models;
using TMS.Domain.RefreshTokens;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Config;

public class StudentConfiguration: IEntityTypeConfiguration<Student>
{
	public void Configure(EntityTypeBuilder<Student> builder)
	{
		builder.HasKey(s => s.Id);
		builder.Property(p => p.Id)
			.HasConversion(
				v => v.Value,
				v => StudentId.Create(v)
			);
		builder.ToTable("Students");
		// Configure properties
		builder.Property(s => s.Name).IsRequired();
		builder.Property(s => s.Gender).IsRequired();
		builder.Property(s => s.Email);
		builder.Property(s => s.Phone);
		// Configure relationships



		builder.HasMany(s => s.Accounts)
			.WithOne(a => a.Student)
			.HasForeignKey(a => a.StudentId)
			.OnDelete(DeleteBehavior.NoAction);
		
		// Configure indexes
		
		builder.HasMany<RefreshToken>()
			.WithOne()
			.HasForeignKey(rt => rt.StudentId);
		builder.HasIndex(s => s.Phone).IsUnique();
		builder.HasIndex(s => s.Email).IsUnique();
		

		

	}
}