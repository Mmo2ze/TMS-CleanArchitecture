using ErrorOr;
using MediatR;
using TMS.Application.TeacherApp.Queries.GetTeacher;

namespace TMS.Contracts.Teacher.GetTeacher;

public record GetTeacherRequest(string Id):IRequest<ErrorOr<GetTeacherResult>>;
