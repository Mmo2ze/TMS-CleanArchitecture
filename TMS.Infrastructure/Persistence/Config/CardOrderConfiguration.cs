using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Accounts;
using TMS.Domain.Admins;
using TMS.Domain.Cards;
using TMS.Domain.Common.Errors;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Config;

public class CardOrderConfiguration : IEntityTypeConfiguration<CardOrder>
{
    public void Configure(EntityTypeBuilder<CardOrder> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new CardOrderId(x));



        //relationships
        builder.OwnsMany(m => m.AccountIds, ab =>
        {
            ab.ToTable("CardOrderAccountIds");
            ab.WithOwner()
                .HasForeignKey("CardOrderId");
            ab.HasKey("Id");
            ab.Property(a => a.Value)
                .HasColumnName("AccountId")
                .ValueGeneratedNever();
        });
        builder.Metadata.FindNavigation(nameof(CardOrder.AccountIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.HasOne<Teacher>()
            .WithMany()
            .HasForeignKey(x => x.TeacherId);
        builder.HasOne<Admin>()
            .WithMany()
            .HasForeignKey(x => x.AcceptedBy);
        builder.HasOne<Admin>()
            .WithMany()
            .HasForeignKey(x => x.CancelledBy);
    }
}