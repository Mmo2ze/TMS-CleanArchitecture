using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Repositories;
using TMS.Domain.RefreshTokens;

namespace TMS.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository:IRefreshTokenRepository
{
    private readonly MainContext _context;

    public RefreshTokenRepository(MainContext context)
    {
        _context = context;
    }

    public Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
       _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
       return Task.CompletedTask;
    }

    public Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }

    public async Task<bool> DeleteRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = await GetRefreshTokenAsync(token, cancellationToken);
        if (refreshToken is null)
            return false;
        _context.RefreshTokens.Remove(refreshToken);
        return true;
    }

}