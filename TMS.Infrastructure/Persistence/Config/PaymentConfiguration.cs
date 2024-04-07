using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Students;

namespace TMS.Infrastructure.Persistence.Config;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
	public void Configure(EntityTypeBuilder<Payment> builder)
	{
		builder.HasKey(p => p.Id);
		builder.ToTable("StudentPayments");
		builder.Property(p => p.Id)
			.HasConversion(
				v => v.Value,
				v => PaymentId.Create(v)
			);

		// Configure properties
		builder.Property(p => p.Amount).IsRequired();
		builder.Property(p => p.CreatedAt).IsRequired();
		builder.Property(p => p.BillDate).IsRequired();
		

		

	}
}