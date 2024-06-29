using TMS.Domain.Assistants;
using TMS.Domain.Payments;

namespace TMS.Application.Payments.Commands.Create;

public record PaymentResult(
    PaymentId Id,
    decimal Amount,
    DateOnly BillDate,
    AssistantInfo CreatedBy,
    DateTime CreatedAt,
    AssistantInfo? UpdatedBy = null,
    DateTime? UpdatedAt = null);

