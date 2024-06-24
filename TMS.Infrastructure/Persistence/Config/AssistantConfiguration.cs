using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Models;
using TMS.Domain.RefreshTokens;
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
		builder.Property(a => a.Name).IsRequired().HasMaxLength(Constrains.Assistant.Name.Max);
		builder.Property(a => a.Email).HasMaxLength(Constrains.Email.Max);
		builder.Property(a => a.Phone).IsRequired().HasMaxLength(Constrains.Phone.Max);
		
		// Configure relationships
		
		builder.HasMany<RefreshToken>()
			.WithOne()
			.HasForeignKey(rt => rt.AssistantId).OnDelete(DeleteBehavior.Cascade);
		builder.Property(x => x.RolesString).HasMaxLength(560);
		builder.Ignore(x => x.Roles);

		
		// Configure index
		builder.HasIndex(s => s.Phone).IsUnique();
		builder.HasIndex(s => s.Email).IsUnique();
	}
}