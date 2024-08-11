using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Accounts;
using TMS.Domain.Attendances;

namespace TMS.Infrastructure.Persistence.Config;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(
            v => v.Value,
            v => AccountId.Create(v));

        builder.Property(x => x.BasePrice).IsRequired();
        builder.Property(x => x.AttendanceStatus).HasConversion(
            v => v != null ? v.ToString() : null,
            v => v != null ? Enum.Parse<AttendanceStatus>(v) : null);
    }
}