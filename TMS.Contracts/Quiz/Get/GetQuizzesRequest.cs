using TMS.Application.Common;

namespace TMS.Contracts.Quiz.Get;

public record GetQuizzesRequest(string AccountId, int PageNumber = 1, int PageSize = 10): GetPaginatedList
    (PageNumber, PageSize);