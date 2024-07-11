using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.RefreshTokens;
using TMS.Domain.Students;

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
        builder.Property(x => x.StudentId).HasConversion(
            v => v.Value,
            v => new StudentId(v));
    }
}