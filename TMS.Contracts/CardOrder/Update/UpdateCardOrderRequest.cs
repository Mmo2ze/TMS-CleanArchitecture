using TMS.Domain.Cards;

namespace TMS.Contracts.CardOrder.Update;

public record UpdateCardOrderRequest(string Id, List<string>? Ids, CardOrderStatus Status);