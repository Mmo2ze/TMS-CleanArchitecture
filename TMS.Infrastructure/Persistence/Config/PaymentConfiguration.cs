using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Accounts;
using TMS.Domain.Payments;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Config;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(v =>
                v.Value,
            v => new PaymentId(v)
        );

        //relationships
        builder.HasOne<Teacher>()
            .WithMany()
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Account>( )
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => new { x.AccountId, x.BillDate }).IsUnique();

    }
}