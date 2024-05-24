using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Groups;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Config;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(
            v => v.Value,
            v => new AccountId(v));

        builder.Property(x => x.BasePrice).IsRequired();


                
    }
}