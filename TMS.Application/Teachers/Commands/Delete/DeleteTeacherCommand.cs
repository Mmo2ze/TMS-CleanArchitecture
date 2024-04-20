using ErrorOr;
using MediatR;
using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Commands.Delete;

public record DeleteTeacherCommand(TeacherId Id):IRequest<ErrorOr<DeleteTeacherResult>>;