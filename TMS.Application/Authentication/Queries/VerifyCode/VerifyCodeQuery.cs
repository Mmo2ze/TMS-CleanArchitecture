using ErrorOr;
using MediatR;
using TMS.Application.Common.Results.Auth;

namespace TMS.Application.Authentication.Queries;

public record VerifyCodeQuery(string Code) : IRequest<ErrorOr<VerifyCodeResult>>;
