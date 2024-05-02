using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Models;
using TMS.Domain.Students;

namespace TMS.Infrastructure.Persistence.Config;

public class AssistantConfiguration: IEntityTypeConfiguration<Assistant>
{
	public void Configure(EntityTypeBuilder<Assistant> builder)
	{
		builder.HasKey(a => a.Id);
		builder.ToTable("Assistants");
		
		// Configure properties
		builder.Property(a => a.Id)
			.HasConversion(
				v => v.Value,
				v => AssistantId.Create(v)
			);
		builder.Property(a => a.Name).IsRequired().HasMaxLength(26);
		builder.Property(a => a.Email).HasMaxLength(128);
		builder.Property(a => a.Phone).IsRequired().HasMaxLength(16);
		
		// Configure relationships
		builder.HasMany<Payment>()
			.WithOne()
			.HasForeignKey(p => p.AssistantId);
		builder.HasMany<RefreshToken>()
			.WithOne()
			.HasForeignKey(rt => rt.AssistantId);
		// Configure index
		builder.HasIndex(s => s.Phone).IsUnique();
		builder.HasIndex(s => s.Email).IsUnique();
	}
}