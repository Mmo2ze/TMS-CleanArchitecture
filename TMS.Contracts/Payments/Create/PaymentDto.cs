using TMS.Domain.Assistants;

namespace TMS.Contracts.Payments.Create;

public record PaymentDto(
    string Id,
    decimal Amount,
    DateOnly BillDate,
    AssistantInfoDto CreatedBy,
    DateTime CreatedAt,
    AssistantInfoDto? UpdatedBy = null,
    DateTime? UpdatedAt = null);
    
public record AssistantInfoDto(string Name, string Id);