using ErrorOr;
using MediatR;

namespace TMS.Application.Teachers.Queries.GetTeachers;

public record GetTeachersQuery(int Page, int PageSize) : IRequest<ErrorOr<GetTeachersResult>>, IRequest<GetTeachersResult>;