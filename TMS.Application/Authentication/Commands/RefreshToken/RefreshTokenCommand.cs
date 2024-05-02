using ErrorOr;
using MediatR;
using TMS.Application.Authentication.Common;

namespace TMS.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand():IRequest<ErrorOr<AuthenticationResult>>;
