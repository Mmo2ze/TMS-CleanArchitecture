using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ErrorOr;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TMS.Application.Authentication.Common;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Domain.Admins;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Enums;
using TMS.Domain.Common.Errors;
using TMS.Domain.Parents;
using TMS.Domain.RefreshTokens;
using TMS.Domain.Students;
using TMS.Domain.Teachers;
using TMS.Infrastructure.Persistence;

namespace TMS.Infrastructure.Auth;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;
    private readonly ICookieManger _cookieManger;
    private readonly MainContext _dbContext;
    private readonly IDistributedCache _cache;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions,
        ICookieManger cookieManger, MainContext dbContext, IDistributedCache cache)
    {
        _dateTimeProvider = dateTimeProvider;
        _cookieManger = cookieManger;
        _dbContext = dbContext;
        _cache = cache;
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateJwtToken(List<Claim> claims, DateTime expireTime, UserAgent agent,
        string? userId)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256);
        var jwtExpireMinutes = _jwtSettings.ExpireMinutes;
        var jwtExpireTime = _dateTimeProvider.Now.AddMinutes(jwtExpireMinutes);
        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            claims: claims,
            audience: _jwtSettings.Audience,
            expires: jwtExpireTime,
            signingCredentials: signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public string GenerateJwtToken(List<Claim> claims, TimeSpan period, UserAgent agent, string userId)
    {
        return GenerateJwtToken(claims, _dateTimeProvider.Now.Add(period), agent, userId);
    }

    public async Task<ErrorOr<AuthenticationResult>> RefreshToken(List<Claim> claims, TimeSpan expireTime,
        UserAgent agent,
        string userId, RefreshToken? oldToken, CancellationToken cancellationToken = default)
    {
        var token = GenerateJwtToken(claims, TimeSpan.FromMinutes(_jwtSettings.ExpireMinutes), agent, userId);
        var baseRefreshToken = GenerateRefreshToken(expireTime);
        List<Role> roles = [];
        if (claims.Any(x => x.Value == Role.CodeSent.ToString()))
            roles.Add(Role.CodeSent);
        else
        {
            switch (agent)
            {
                case UserAgent.Admin:
                    var adminId = new AdminId(userId);
                    var admin = _dbContext.Admins.FirstOrDefault(a => a.Id == adminId);
                    if (admin is null)
                        return Errors.Auth.InvalidCredentials;
                    baseRefreshToken.AdminId = admin.Id;
                    roles.Add(Role.Admin);
                    break;
                case UserAgent.Teacher:
                    if (TeacherId.IsValidId(userId))
                    {
                        var teacher = await _dbContext.Teachers.FindAsync(new object?[] { new TeacherId(userId) },
                            cancellationToken: cancellationToken);
                        if (teacher is null)
                            return Errors.Auth.InvalidCredentials;
                        baseRefreshToken.TeacherId = teacher.Id;
                        roles.Add(Role.Teacher);
                    }
                    else
                    {
                        var assistant = _dbContext.Assistants.Find(new AssistantId(userId));
                        if (assistant is null)
                            return Errors.Auth.InvalidCredentials;
                        baseRefreshToken.AssistantId = assistant.Id;
                        roles.Add(Role.Assistant);
                        roles.AddRange(assistant.Roles.ToBasicRoleList());
                    }

                    break;
                case UserAgent.Student:
                    var student = await _dbContext.Students.FindAsync(new object?[] { new StudentId(userId) },
                        cancellationToken: cancellationToken);
                    if (student is null)
                        return Errors.Auth.InvalidCredentials;
                    baseRefreshToken.StudentId = student.Id;
                    roles.Add(Role.Student);

                    break;
                case UserAgent.Parent:
                    var parent = _dbContext.Parents.Find(new ParentId(userId));
                    if (parent is null)
                        return Errors.Auth.InvalidCredentials;
                    baseRefreshToken.ParentId = parent.Id;
                    roles.Add(Role.Parent);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(agent), agent, null);
            }
        }

        await using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                _dbContext.RefreshTokens.Add(baseRefreshToken);
                if (oldToken is not null)
                {
                    _dbContext.Remove(oldToken);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                if (oldToken != null) await _cache.RemoveAsync(oldToken.Token, cancellationToken);
                await _cache.SetRecordAsync(baseRefreshToken.Token, baseRefreshToken, baseRefreshToken.Duration);
            }
            catch (OperationCanceledException ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                if (oldToken is not null)
                    _cookieManger.SetProperty(CookieVariables.RefreshToken, oldToken.Token, expireTime);
                _cookieManger.SetProperty(CookieVariables.Agent, agent.ToString(), expireTime);
                _cookieManger.SetProperty(CookieVariables.Id, userId, expireTime);
                _cookieManger.SetProperty(CookieVariables.Autorized,
                    roles.Any(x => x.ToString() == Role.CodeSent.ToString()) ? "false" : "true", expireTime);
                Console.WriteLine("request was canceled" + ex.Message);
                return Error.Unexpected("Error while saving refresh token.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                if (oldToken is not null)
                    _cookieManger.SetProperty(CookieVariables.RefreshToken, oldToken.Token, expireTime);
                _cookieManger.SetProperty(CookieVariables.Agent, agent.ToString(), expireTime);
                _cookieManger.SetProperty(CookieVariables.Id, userId, expireTime);
                _cookieManger.SetProperty(CookieVariables.Autorized,
                    roles.Any(x => x.ToString() == Role.CodeSent.ToString()) ? "false" : "true", expireTime);
                Console.WriteLine("saving refresh token failed. " + ex.Message);
                return Error.Unexpected("Error while saving refresh token.");
            }
        }


        _cookieManger.SetProperty(CookieVariables.RefreshToken, baseRefreshToken.Token, expireTime);
        _cookieManger.SetProperty(CookieVariables.Agent, agent.ToString(), expireTime);
        _cookieManger.SetProperty(CookieVariables.Id, userId, expireTime);
        _cookieManger.SetProperty(CookieVariables.Autorized,
            roles.Any(x => x.ToString() == Role.CodeSent.ToString()) ? "false" : "true", expireTime);

        return new AuthenticationResult(token, baseRefreshToken.Expires, roles);
    }

    private RefreshToken GenerateRefreshToken(TimeSpan expireTime)
    {
        var randomNumber = new byte[32];

        using var generator = new RNGCryptoServiceProvider();

        generator.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            Expires = DateTime.UtcNow.Add(expireTime),
            CreatedOn = DateTime.UtcNow,
            TokenId = Guid.NewGuid()
        };
    }
}