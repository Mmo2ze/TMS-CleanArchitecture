using ErrorOr;
using MediatR;
using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Commands.Update;

public record UpdateTeacherPartialCommand(
    TeacherId TeacherId,
    string? Name,
    string? Phone,
    Subject? Subject,
    string? Email,
    TeacherStatus? Status) : IRequest<ErrorOr<TeacherSummary>>;