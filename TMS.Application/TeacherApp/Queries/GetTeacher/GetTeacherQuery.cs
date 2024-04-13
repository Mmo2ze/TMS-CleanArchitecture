using ErrorOr;
using MediatR;
using TMS.Domain.Teachers;

namespace TMS.Application.TeacherApp.Queries.GetTeacher;

public abstract record GetTeacherQuery(TeacherId Id):IRequest<ErrorOr<GetTeacherResult>>;