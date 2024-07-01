using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Admins;
using TMS.Domain.Cards;
using TMS.Domain.Common.Models;
using TMS.Domain.Teachers;

namespace TMS.Application.CardOrders.Queries.Get.Order;

public record GetCardOrderQuery(CardOrderId Id) : IRequest<ErrorOr<CardOrderDetailsResult>>;

public record CardOrderDetailsResult(
    CardOrderId Id,
    
    TeacherId TeacherId,
    string TeacherName,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    DateTime? CancelledAt,
    AdminId? AcceptedBy,
    AdminId? CancelledBy,
    int Count,
    CardOrderStatus Status,
    List<ShortAccount> ShortAccounts)
{
    public List<ShortAccount> AccountCards { get; set; } = ShortAccounts;
};