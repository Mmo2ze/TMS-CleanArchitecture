using ErrorOr;
using MediatR;
using TMS.Application.Teachers.Queries.GetTeacher;

namespace TMS.Contracts.Teacher.GetTeacher;

public record GetTeacherRequest(string Id);
