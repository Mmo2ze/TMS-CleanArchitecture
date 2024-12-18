using TMS.Domain.Common.Enums;

namespace TMS.Application.Authentication.Common;

public record AuthenticationResult(
    string Token,
    DateTime ExpireDate,List<Role> Roles,bool IsRegistered =true);