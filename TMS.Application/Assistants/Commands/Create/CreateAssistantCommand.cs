using ErrorOr;
using MediatR;
using TMS.Domain.Assistants;

namespace TMS.Application.Assistants.Commands.Create;

public record CreateAssistantCommand(string Phone, string Name, string? Email,List<AssistantRole> Roles)
    : IRequest<ErrorOr<AssistantDto>>;