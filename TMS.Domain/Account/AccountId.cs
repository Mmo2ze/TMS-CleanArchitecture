namespace TMS.Domain.Account;

public record AccountId(string Value) : ValueObjectId<AccountId>(Value)
{
    public AccountId() : this(Guid.NewGuid().ToString())
    {
    }
}