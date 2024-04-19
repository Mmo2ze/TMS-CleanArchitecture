using ErrorOr;
using MediatR;
using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Queries.GetTeacher;

public abstract record GetTeacherQuery(TeacherId Id):IRequest<ErrorOr<GetTeacherResult>>;