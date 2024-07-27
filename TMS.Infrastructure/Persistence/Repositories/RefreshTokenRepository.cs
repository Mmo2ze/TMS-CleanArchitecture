using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using TMS.Application.Common.Extensions;
using TMS.Domain.Common.Repositories;
using TMS.Domain.RefreshTokens;

namespace TMS.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly MainContext _context;
    private readonly IDistributedCache _cache;

    public RefreshTokenRepository(MainContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        _ = _cache.SetRecordAsync(refreshToken.Token, refreshToken, refreshToken.Duration);
        return Task.CompletedTask;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        //get from cache
        var refreshToken = await _cache.GetRecordAsync<RefreshToken>(token);
        
        if (refreshToken is not null) 
            return refreshToken;
        
        // get from db 
        refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(r => r.Token == token, cancellationToken);
        
        // set to cache
        if (refreshToken is not null)
            await _cache.SetRecordAsync(token, refreshToken, refreshToken.Duration);

        return refreshToken;
    }

    public async Task<bool> DeleteRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = await GetRefreshTokenAsync(token, cancellationToken);
        if (refreshToken is null)
            return false;
        await _cache.RemoveAsync(token, cancellationToken);
        _context.RefreshTokens.Remove(refreshToken);
        return true;
    }
}