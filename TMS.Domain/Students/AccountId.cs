using TMS.Domain.Common.Models;

namespace TMS.Domain.Students;

public record AccountId(string Value) : ValueObjectId<AccountId>(Value)
{
    public AccountId() : this(Guid.NewGuid().ToString())
    {
    }
}