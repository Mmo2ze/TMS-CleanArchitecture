using TMS.Application.CardOrders.Queries.Get.Order;

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
    List<ShortAccountDto> ShortAccounts);

public record ShortAccountDto(string Id, string Name);