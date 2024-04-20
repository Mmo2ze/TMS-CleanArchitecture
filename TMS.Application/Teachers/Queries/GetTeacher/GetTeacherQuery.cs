using ErrorOr;
using MediatR;
using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Queries.GetTeacher;

public  record GetTeacherQuery(TeacherId Id):IRequest<ErrorOr<GetTeacherResult>>;