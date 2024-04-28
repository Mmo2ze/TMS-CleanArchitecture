using ErrorOr;
using MediatR;
using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Commands.Update;

public record UpdateTeacherCommand(
    TeacherId TeacherId,
    string Name,
    string Phone,
    Subject Subject,
    TeacherStatus Status,
    string? Email) : IRequest<ErrorOr<TeacherSummary>>;