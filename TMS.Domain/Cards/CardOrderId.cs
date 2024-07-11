namespace TMS.Domain.Cards;

public record CardOrderId(string Value) : ValueObjectId<CardOrderId>(Value)
{
    public CardOrderId() : this(Guid.NewGuid().ToString())
    {
    }
}