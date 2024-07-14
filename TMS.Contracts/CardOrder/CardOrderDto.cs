namespace TMS.Contracts.CardOrder;

public record CardOrderDto(
    string Id,
    string TeacherId,
    string TeacherName,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    DateTime? CancelledAt,
    string AcceptedBy,
    string CancelledBy,
    int Count,
    string Status
);