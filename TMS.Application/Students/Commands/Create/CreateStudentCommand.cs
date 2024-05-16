using ErrorOr;
using MediatR;
using TMS.Domain.Common.Enums;
using TMS.Domain.Parents;
using TMS.Domain.Students;

namespace TMS.Application.Students.Commands.Create;

public record CreateStudentCommand(
    string Name,
    Gender Gender,
    ParentId? ParentId = null,
    string? Email = null,
    string? Phone = null) : IRequest<ErrorOr<StudentId>>;