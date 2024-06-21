using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Models;

namespace TMS.Contracts.Student.Get;

public record GetStudentResponse(PaginatedList<StudentDto> Students);