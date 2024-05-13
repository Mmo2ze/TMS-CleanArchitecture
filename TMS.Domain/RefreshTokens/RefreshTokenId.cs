namespace TMS.Domain.RefreshTokens;

public record RefreshTokenId(string Value) : ValueObjectId<RefreshTokenId>(Value)
{
    public RefreshTokenId() : this(Guid.NewGuid().ToString())
    {
    }
}