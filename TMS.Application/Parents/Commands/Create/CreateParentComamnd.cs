using ErrorOr;
using MediatR;
using TMS.Domain.Common.Enums;
using TMS.Domain.Parents;

namespace TMS.Application.Parents.Commands.Create;

public record CreateParentComamnd(
    string Name,
    Gender Gender,
    string? Email = null,
    string? Phone = null): IRequest<ErrorOr<ParentId>>;