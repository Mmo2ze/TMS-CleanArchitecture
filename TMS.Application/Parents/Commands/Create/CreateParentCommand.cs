using ErrorOr;
using MediatR;
using TMS.Application.Parents.Queries.Get;
using TMS.Domain.Common.Enums;
using TMS.Domain.Parents;

namespace TMS.Application.Parents.Commands.Create;

public record CreateParentCommand(
    string Name,
    Gender Gender,
    string? Email = null,
    string? Phone = null): IRequest<ErrorOr<ParentResult>>;