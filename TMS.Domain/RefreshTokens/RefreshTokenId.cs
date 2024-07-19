namespace TMS.Domain.RefreshTokens;

public record RefreshTokenId(string Value) : ValueObjectId<RefreshTokenId>(Value)
{
    public RefreshTokenId() : this(Ulid.NewUlid().ToString())
    {
    }
}