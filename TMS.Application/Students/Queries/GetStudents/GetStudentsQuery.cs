using ErrorOr;
using MediatR;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Models;
using TMS.Domain.Students;

namespace TMS.Application.Students.Queries.GetStudents;

public record GetStudentsQuery(
    int Page,
    int PageSize,
    string? Search = null,
    StudentSort? Sort = null,
    bool Asc = false,
    bool PhoneRequired = true)
    : IRequest<ErrorOr<PaginatedList<StudentResult>>>;

public enum StudentSort
{
    Name,
}