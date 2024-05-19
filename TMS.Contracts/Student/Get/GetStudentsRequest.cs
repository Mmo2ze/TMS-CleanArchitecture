using TMS.Application.Students.Queries.GetStudents;

namespace TMS.Contracts.Student.Get;

public record GetStudentsRequest(int Page, int PageSize, string? Search = null, StudentSort? Sort = null, bool Asc = false);
