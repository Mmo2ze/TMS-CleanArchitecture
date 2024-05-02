using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository:IRefreshTokenRepository
{
    private readonly MainContext _context;

    public RefreshTokenRepository(MainContext context)
    {
        _context = context;
    }

    public Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
       _context.RefreshTokens.Add(refreshToken);
       return Task.CompletedTask;
    }

    public Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<bool> DeleteRefreshTokenAsync(string token)
    {
        var refreshToken = await GetRefreshTokenAsync(token);
        if (refreshToken is null)
            return false;
        _context.RefreshTokens.Remove(refreshToken);
        return true;
    }

}