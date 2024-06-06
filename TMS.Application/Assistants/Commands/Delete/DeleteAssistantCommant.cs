using ErrorOr;
using MediatR;
using TMS.Domain.Assistants;

namespace TMS.Application.Assistants.Commands.Delete;

public record DeleteAssistantCommand(AssistantId Id): IRequest<ErrorOr<string>>;