using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Admins;

namespace TMS.Infrastructure.Persistence.Config;

public class AdminConfiguration:IEntityTypeConfiguration<Admin>
{
	public void Configure(EntityTypeBuilder<Admin> builder)
	{
		builder.ToTable("Admins");
		builder.HasKey(a => a.Id);
		// Configure properties
		builder.Property(a => a.Id).HasConversion(
			v => v.Value,
			v => AdminId.Create(v)
		);
		builder.Property(a => a.Name).IsRequired().HasMaxLength(26);
		builder.Property(a => a.Email).HasMaxLength(128);
		builder.Property(a => a.Phone).IsRequired().HasMaxLength(16);
		
		// Configure index
		builder.HasIndex(s => s.Phone).IsUnique();
		builder.HasIndex(s => s.Email).IsUnique();
	}
}