using TMS.Contracts.Account.DTOs;

namespace TMS.Contracts.CardOrder.Get;

public record CardOrderDetailsDto(
    string Id,
    
    string TeacherId,
    string TeacherName,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    DateTime? CancelledAt,
    string AcceptedBy,
    string CancelledBy,
    int Count,
    string Status,
    List<AccountSummaryDto> Accounts);

