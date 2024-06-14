using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using TMS.Application.Authentication.Common;

namespace TMS.Application.Authentication.Queries.VerifyCode;

public record VerifyCodeQuery(string Code) : IRequest<ErrorOr<AuthenticationResult>>;
