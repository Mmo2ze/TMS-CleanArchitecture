using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ErrorOr;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TMS.Application.Authentication.Common;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Domain.Admins;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Models;
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

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions,
        ICookieManger cookieManger, MainContext dbContext)
    {
        _dateTimeProvider = dateTimeProvider;
        _cookieManger = cookieManger;
        _dbContext = dbContext;
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

    public ErrorOr<AuthenticationResult> RefreshToken(List<Claim> claims, TimeSpan expireTime, UserAgent agent,
        string userId)
    {
        var token = GenerateJwtToken(claims, TimeSpan.FromMinutes(_jwtSettings.ExpireMinutes), agent, userId);
        var baseRefreshToken = GenerateRefreshToken(expireTime);
        switch (agent)
        {
            case UserAgent.Admin:
                var adminId = new AdminId(userId);
                var admin = _dbContext.Admins.FirstOrDefault(a => a.Id == adminId);
                if (admin is null)
                    return Errors.Auth.InvalidCredentials;
                baseRefreshToken.AdminId = admin.Id;
                break;
            case UserAgent.Teacher:
                if (TeacherId.IsValidId(userId))
                {
                    var teacher = _dbContext.Teachers.Find(new TeacherId(userId));
                    if (teacher is null)
                        return Errors.Auth.InvalidCredentials;
                    baseRefreshToken.TeacherId = teacher.Id;
                }
                else
                {
                    var assistant = _dbContext.Assistants.Find(new AssistantId(userId));
                    if (assistant is null)
                        return Errors.Auth.InvalidCredentials;
                    baseRefreshToken.AssistantId = assistant.Id;
                }

                break;
            case UserAgent.Student:
                var student = _dbContext.Students.Find(new StudentId(userId));
                if (student is null)
                    return Errors.Auth.InvalidCredentials;
                baseRefreshToken.StudentId = student.Id;
                break;
            case UserAgent.Parent:
                var parent = _dbContext.Parents.Find(new ParentId(userId));
                if (parent is null)
                    return Errors.Auth.InvalidCredentials;
                baseRefreshToken.ParentId = parent.Id;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(agent), agent, null);
        }

        try
        {
            _dbContext.RefreshTokens.Add(baseRefreshToken);
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            return Error.Unexpected("unexpected error occurred while saving refresh token");
        }

        _cookieManger.SetProperty(CookieVariables.RefreshToken, baseRefreshToken.Token, expireTime);
        _cookieManger.SetProperty(CookieVariables.Agent, agent.ToString(), expireTime);
        _cookieManger.SetProperty(CookieVariables.Id, userId, expireTime);

        return new AuthenticationResult(token, baseRefreshToken.Expires);
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