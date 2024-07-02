using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Admins;
using TMS.Domain.Cards;
using TMS.Domain.Teachers;

namespace TMS.Application.CardOrders.Commands.Create;

public record CreateCardOrderCommand(List<AccountId> AccountIds) : IRequest<ErrorOr<CardOrderResult>>;

public record CardOrderResult(
    CardOrderId Id,
    IReadOnlyList<AccountId> AccountIds,
    TeacherId TeacherId,
    string TeacherName,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    DateTime? CancelledAt,
    AdminId? AcceptedBy,
    AdminId? CancelledBy,
    CardOrderStatus Status)
{
    public static CardOrderResult From(CardOrder cardOrder)
    {
        return new CardOrderResult(
            cardOrder.Id,
            cardOrder.Accounts.Select(x => x.Id).ToList(),
            cardOrder.TeacherId,
            cardOrder.TeacherName,
            cardOrder.CreatedAt,
            cardOrder.AcceptedAt,
            cardOrder.CancelledAt,
            cardOrder.AcceptedBy,
            cardOrder.CancelledBy,
            cardOrder.Status);
    }

    public int Count = AccountIds.Count;
}