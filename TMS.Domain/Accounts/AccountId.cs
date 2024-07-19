namespace TMS.Domain.Accounts;

public record AccountId(string Value) : ValueObjectId<AccountId>(Value)
{
    public AccountId() : this(Ulid.NewUlid().ToString())
    {
    }
}