using ErrorOr;
using MediatR;
using TMS.Application.Students.Queries.GetStudents;
using TMS.Domain.Common.Enums;
using TMS.Domain.Parents;
using TMS.Domain.Students;

namespace TMS.Application.Students.Commands.Create;

public record CreateStudentCommand(
    string Name,
    Gender Gender,
    string? Email = null,
    string? Phone = null) : IRequest<ErrorOr<StudentResult>>;