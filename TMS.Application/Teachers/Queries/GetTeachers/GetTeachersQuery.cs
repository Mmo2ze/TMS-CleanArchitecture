using MediatR;
using TMS.Domain.Common.Models;
using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Queries.GetTeachers;

public record GetTeachersQuery(int Page, int PageSize)
    :  IRequest<PaginatedList<TeacherSummary>>;