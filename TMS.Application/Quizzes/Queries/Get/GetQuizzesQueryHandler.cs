using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Mapping;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Quizzes.Queries.Get;

public class GetQuizzesQueryHandler : IRequestHandler<GetQuizzesQuery, ErrorOr<PaginatedList<QuizResult>>>
{
    private readonly IQuizRepository _quizRepository;

    public GetQuizzesQueryHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<PaginatedList<QuizResult>>> Handle(GetQuizzesQuery request,
        CancellationToken cancellationToken)
    {
        var quizzes = _quizRepository.GetQueryable().Where(a => a.AccountId == request.AccountId)
            .Include(x => x.AddedBy)
            .Include(x => x.UpdatedBy)
            .Select(q => QuizResult.From(q));

        return await quizzes
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}