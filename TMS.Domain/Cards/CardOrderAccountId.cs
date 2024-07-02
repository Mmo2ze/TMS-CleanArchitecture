using TMS.Domain.Accounts;

namespace TMS.Domain.Cards;

public class CardOrderAccountId
{
    public CardOrderAccountId( CardOrderId cardOrderId, AccountId accountId)
    {
        CardOrderId = cardOrderId;
        AccountId = accountId;
    }

    public int Id { get; set; }
    public CardOrderId CardOrderId { get; set; }
    public AccountId AccountId { get; set; }
}