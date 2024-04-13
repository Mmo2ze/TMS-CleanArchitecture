using ErrorOr;
using MediatR;
using TMS.Application.Common.Enums;

namespace TMS.Application.Authentication.Commands.SendCode;

public record SendCodeCommand(string Phone,UserAgent UserAgent) : IRequest<ErrorOr<SendCodeResult>>;