namespace TMS.Application.Authentication.Common;

public record AuthenticationResult(
    string Token,
    DateTime ExpireDate);