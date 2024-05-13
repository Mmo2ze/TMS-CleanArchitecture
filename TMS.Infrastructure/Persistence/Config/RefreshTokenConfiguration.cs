using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Common.Models;
using TMS.Domain.RefreshTokens;

namespace TMS.Infrastructure.Persistence.Config;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.TokenId);
        builder.ToTable("RefreshTokens");
        builder.Property(rt => rt.Token).IsRequired();
        builder.Property(rt => rt.TokenId).IsRequired();
        builder.Property(rt => rt.Expires).IsRequired();
        builder.Property(rt => rt.CreatedOn).IsRequired();

    }
}