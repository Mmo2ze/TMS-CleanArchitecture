﻿using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using TMS.Application.Authentication.Common;
using TMS.Application.Common.Enums;

namespace TMS.Application.Authentication.Commands.SendCode;

public record SendCodeCommand(string Phone,UserAgent UserAgent) : IRequest<ErrorOr<AuthenticationResult>>;