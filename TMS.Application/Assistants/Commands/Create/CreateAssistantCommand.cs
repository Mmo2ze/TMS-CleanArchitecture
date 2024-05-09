using ErrorOr;
using MediatR;

namespace TMS.Application.Assistants.Commands.Create;

public record CreateAssistantCommand(string Phone, string Name, string? Email)
    : IRequest<ErrorOr<CreateAssistantResult>>;