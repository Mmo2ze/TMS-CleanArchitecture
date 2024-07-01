using ErrorOr;
using MediatR;
using TMS.Application.CardOrders.Commands.Create;
using TMS.Domain.Accounts;
using TMS.Domain.Cards;

namespace TMS.Application.CardOrders.Commands.UpdateCardOrderCommand;

public record UpdateCardOrderCommand(CardOrderId Id, List<AccountId>? Ids, CardOrderStatus Status): IRequest<ErrorOr<CardOrderResult>>;