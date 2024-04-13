using ErrorOr;
using MediatR;

namespace TMS.Application.TeacherApp.Queries.GetTeachers;

public record GetTeachersQuery(int Page, int PageSize) : IRequest<ErrorOr<GetTeachersResult>>, IRequest<GetTeachersResult>;