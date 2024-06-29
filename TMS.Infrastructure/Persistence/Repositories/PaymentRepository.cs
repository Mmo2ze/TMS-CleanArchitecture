using TMS.Domain.Common.Repositories;
using TMS.Domain.Payments;

namespace TMS.Infrastructure.Persistence.Repositories;

public class PaymentRepository: Repository<Payment,PaymentId>,IPaymentRepository
{
    public PaymentRepository(MainContext dbContext) : base(dbContext)
    {
    }
}