using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Models;
using TMS.Domain.Parents;

namespace TMS.Infrastructure.Persistence.Config;

public class ParentConfiguration: IEntityTypeConfiguration<Parent>
{
	public void Configure(EntityTypeBuilder<Parent> builder)
	{
		builder.HasKey(p => p.Id);
		builder.ToTable("Parents");
		
		// Configure properties
		builder.Property(p => p.Id)
			.HasConversion(
				v => v.Value,
				v => ParentId.Create(v)
			);
		builder.Property(p => p.Name).IsRequired().HasMaxLength(Constrains.Parent.NameMaxLength);
		builder.Property(p => p.Email).HasMaxLength(Constrains.Email.Max);
		builder.Property(p => p.Phone).HasMaxLength(Constrains.Phone.Max);
		
		// Configure relationships
		builder.HasMany(p => p.Children)
			.WithOne(s => s.Parent);
		builder.HasMany<RefreshToken>()
			.WithOne()
			.HasForeignKey(rt => rt.ParentId);
		
		// Configure index
		builder.HasIndex(s => s.Phone).IsUnique();
		builder.HasIndex(s => s.Email).IsUnique();

	}
}