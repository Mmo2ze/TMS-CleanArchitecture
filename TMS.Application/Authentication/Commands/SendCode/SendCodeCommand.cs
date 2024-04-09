using ErrorOr;
using MediatR;
using TMS.Application.Common.Enums;
using TMS.Application.Common.Results.Auth;

namespace TMS.Application.Authentication.Commands;

public record SendCodeCommand(string Phone,UserAgent UserAgent) : IRequest<ErrorOr<SendCodeResult>>;