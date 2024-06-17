using TMS.Domain.RefreshTokens;

namespace TMS.Domain.Common.Repositories;

public interface IRefreshTokenRepository
{
    Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<bool> DeleteRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
}