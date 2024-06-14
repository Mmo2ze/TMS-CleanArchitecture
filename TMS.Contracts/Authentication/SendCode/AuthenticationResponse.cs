using TMS.Application.Common.Variables;
using TMS.Domain.Common.Enums;

namespace TMS.Contracts.Authentication.SendCode;

public record AuthenticationResponse(string Token,DateTime ExpireDate,List<Role> Roles,bool IsRegistered =true);