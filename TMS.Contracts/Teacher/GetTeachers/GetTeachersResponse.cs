using TMS.Contracts.Teacher.Common;
using TMS.Domain.Common.Models;

namespace TMS.Contracts.Teacher.GetTeachers;

public class GetTeachersResponse:PaginatedList<TeacherSummaryResponse>
{
	public GetTeachersResponse(IReadOnlyCollection<TeacherSummaryResponse> items, int count, int pageNumber, int pageSize) : base(items, count, pageNumber, pageSize)
	{
	}
}