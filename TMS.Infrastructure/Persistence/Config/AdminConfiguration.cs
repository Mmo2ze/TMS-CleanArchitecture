using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Admins;
using TMS.Domain.Common.Constrains;
using TMS.Domain.RefreshTokens;

namespace TMS.Infrastructure.Persistence.Config;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
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
        builder.Property(a => a.Name).IsRequired().HasMaxLength(Constrains.Admin.Name.Max);
        builder.Property(a => a.Email).HasMaxLength(Constrains.Email.Max);
        builder.Property(a => a.Phone).IsRequired().HasMaxLength(Constrains.Phone.Max);

        builder.HasMany<RefreshToken>()
            .WithOne()
            .HasForeignKey(rt => rt.AdminId);
        // Configure index
        builder.HasIndex(s => s.Phone).IsUnique();
        builder.HasIndex(s => s.Email).IsUnique();
    }
}