using ErrorOr;
using MediatR;
using TMS.Application.Students.Queries.GetStudents;
using TMS.Domain.Common.Enums;

namespace TMS.Application.Students.Commands.Create;

public record CreateStudentCommand(
    string Name,
    Gender Gender,
    string? Email = null,
    string? Phone = null) : IRequest<ErrorOr<StudentResult>>;