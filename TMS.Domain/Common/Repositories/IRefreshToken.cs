using TMS.Domain.RefreshTokens;

namespace TMS.Domain.Common.Repositories;

public interface IRefreshTokenRepository
{
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task<bool> DeleteRefreshTokenAsync(string token);
}