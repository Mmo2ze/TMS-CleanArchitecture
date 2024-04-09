using System.Security.Cryptography.X509Certificates;
using TMS.Application.Common.Enums;

namespace TMS.Contracts.Authentication.SendCode;

public record SendCodeRequest(string Phone,UserAgent UserAgent);