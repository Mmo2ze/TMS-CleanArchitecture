using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Admins;
using TMS.Domain.Cards;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Config;

public class CardOrderConfiguration : IEntityTypeConfiguration<CardOrder>
{
    public void Configure(EntityTypeBuilder<CardOrder> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => CardOrderId.Create(x));


        //relationships
        builder.HasMany(x => x.Accounts)
            .WithMany();
        

        builder.HasOne<Teacher>()
            .WithMany()
            .HasForeignKey(x => x.TeacherId);
        builder.HasOne<Admin>()
            .WithMany()
            .HasForeignKey(x => x.AcceptedBy);
        builder.HasOne<Admin>()
            .WithMany()
            .HasForeignKey(x => x.CancelledBy);
        builder.HasOne<Admin>()
            .WithMany()
            .HasForeignKey(x => x.CompletedBy);
    }
}