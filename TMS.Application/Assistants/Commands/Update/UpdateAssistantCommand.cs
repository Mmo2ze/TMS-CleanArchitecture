using ErrorOr;
using MediatR;
using TMS.Application.Assistants.Commands.Create;
using TMS.Domain.Assistants;

namespace TMS.Application.Assistants.Commands.Update;

public record UpdateAssistantCommand(
    AssistantId Id,
    string Phone,
    string Name,
    string? Email,
    List<AssistantRole> Roles) : IRequest<ErrorOr<AssistantDto>>;