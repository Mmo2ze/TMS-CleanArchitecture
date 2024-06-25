using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Domain.Assistants;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Services;

[Authorize]
public class TeacherHelper : ITeacherHelper
{
    private readonly IClaimsReader _claimsReader;

    public TeacherHelper(IClaimsReader claimsReader)
    {
        _claimsReader = claimsReader;
    }

    public bool IsTeacher()
    {
        var roles = _claimsReader.GetRoles();
        return roles.Contains(Roles.Teacher.Role);
    }

    public bool IsAssistant()
    {
        var roles = _claimsReader.GetRoles();
        return roles.Contains(Roles.Teacher.Assistant);
    }

    public TeacherId GetTeacherId()
    {
        var teacherId = _claimsReader.GetByClaimType(CustomClaimTypes.TeacherId);
        if (teacherId == null || !TeacherId.IsValidId(teacherId))
        {
            throw new Exception("TeacherId is not valid, please login again.");
        }

        return new TeacherId(teacherId);
    }

    public AssistantId? GetAssistantId()
    {
        var assistantId = _claimsReader.GetByClaimType(CustomClaimTypes.Id);
        return assistantId == null || !AssistantId.IsValidId(assistantId)
            ? null
            : new AssistantId(assistantId);
    }

    public AssistantInfo GetAssistantInfo()
    {
        var assistantId = GetAssistantId();
        return new AssistantInfo( _claimsReader.GetByClaimType(ClaimTypes.Name)!, assistantId);
    }

    public string GetTeacherName()
    {
        return _claimsReader.GetByClaimType(ClaimTypes.Name)!;
    }
}