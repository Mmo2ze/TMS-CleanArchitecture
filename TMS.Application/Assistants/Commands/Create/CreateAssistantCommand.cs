using ErrorOr;
using MediatR;
using TMS.Application.Groups.Commands.Create;
using TMS.Domain.Teachers;

namespace TMS.Application.Assistants.Commands.Create;

public record CreateAssistantCommand(string Phone, string Name, string? Email)
    : IRequest<ErrorOr<CreateAssistantResult>>;