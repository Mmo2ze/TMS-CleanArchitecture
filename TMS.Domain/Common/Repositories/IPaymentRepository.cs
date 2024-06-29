using TMS.Domain.Payments;

namespace TMS.Domain.Common.Repositories;

public interface IPaymentRepository : IRepository<Payment, PaymentId>;