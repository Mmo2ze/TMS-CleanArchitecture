using System.Security.Claims;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Variables;
using TMS.Domain.Admins;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Errors;
using TMS.Domain.Teachers;
using TMS.Infrastructure.Persistence;

namespace TMS.Infrastructure.Auth;

public class ClaimGenerator : IClaimGenerator
{
    private readonly MainContext _context;

    public ClaimGenerator(MainContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<List<Claim>>> GenerateClaims(string userId, UserAgent agent)
    {
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.Id, userId),
            new(CustomClaimTypes.Agent, agent.ToString())
        };

        switch (agent)
        {
            case UserAgent.Admin:
                return await GenerateAdminClaims(userId, claims);
            case UserAgent.Teacher:
                return await GenerateTeacherClaims(userId, claims);
            case UserAgent.Student:
                claims.Add(new Claim(ClaimTypes.Role, Roles.Student.Role));
                break;
            case UserAgent.Parent:
                claims.Add(new Claim(ClaimTypes.Role, Roles.Parent.Role));
                break;
            default:
                throw new ArgumentException("Encountered unexpected node, i.e. `agent`");
        }

        return claims;
    }




private async Task<ErrorOr<List<Claim>>> GenerateTeacherClaims(string userId, List<Claim> claims)
{
    if (TeacherId.IsValidId(userId))
    {
        var teacherId = TeacherId.Create(userId);
        var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);
        if (teacher is null)
        {
            return Errors.Auth.InvalidCredentials;
        }

        claims.Add(new Claim(ClaimTypes.Role, Roles.Teacher.Role));
        claims.Add(new Claim(ClaimTypes.MobilePhone, teacher.Phone));
        claims.Add(new Claim(CustomClaimTypes.TeacherId, userId));
        claims.Add(new Claim(ClaimTypes.Name, teacher.Name));

        return claims;
    }

    if (!AssistantId.IsValidId(userId)) return Errors.Auth.InvalidCredentials;

    var assistantId = AssistantId.Create(userId);

    var assistant = _context.Assistants.FirstOrDefault(a => a.Id == assistantId);
    if (assistant is null)
    {
        return Errors.Auth.InvalidCredentials;
    }

    claims.Add(new Claim(ClaimTypes.Role, Roles.Assistant.Role));
    claims.Add(new Claim(ClaimTypes.MobilePhone, assistant.Phone));
    claims.Add(new Claim(CustomClaimTypes.TeacherId, assistant.TeacherId.Value));
    claims.Add(new Claim(ClaimTypes.Name, assistant.Name));
    claims.AddRange(assistant.Roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())));

    return claims;
}

private async Task<ErrorOr<List<Claim>>> GenerateAdminClaims(string userId, List<Claim> claims)
{
    var adminId = AdminId.Create(userId);
    var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == adminId);
    if (admin is null)
    {
        return Errors.Auth.InvalidCredentials;
    }

    claims.Add(new Claim(ClaimTypes.Role, Roles.Admin.Role));
    claims.Add(new Claim(ClaimTypes.MobilePhone, admin.Phone));
    claims.Add(new Claim(ClaimTypes.Name, admin.Name));
    return claims;
}
}