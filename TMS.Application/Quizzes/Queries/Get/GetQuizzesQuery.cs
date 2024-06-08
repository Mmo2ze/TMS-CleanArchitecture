using ErrorOr;
using MediatR;
using TMS.Application.Assistants.Commands.Create;
using TMS.Application.Common;
using TMS.Domain.Account;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Models;

namespace TMS.Application.Quizzes.Queries.Get;

public record GetQuizzesQuery(
    AccountId AccountId,
    int PageNumber = 1,
    int PageSize = 10
) : GetPaginatedList(PageNumber, PageSize), IRequest<ErrorOr<PaginatedList<QuizResult>>>;