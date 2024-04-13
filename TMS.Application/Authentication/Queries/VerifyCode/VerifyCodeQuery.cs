using ErrorOr;
using MediatR;

namespace TMS.Application.Authentication.Queries.VerifyCode;

public record VerifyCodeQuery(string Code) : IRequest<ErrorOr<VerifyCodeResult>>;
