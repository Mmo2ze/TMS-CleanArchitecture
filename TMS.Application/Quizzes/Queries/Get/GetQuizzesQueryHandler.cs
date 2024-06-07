using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
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
                .Select(q => new QuizResult(
                    q.Id,
                    q.Degree,
                    q.MaxDegree,
                    q.AddedBy == null ? null : new QuizAssistantResult(q.AddedBy.Id, q.AddedBy.Name),
                    q.UpdatedBy == null ? null : new QuizAssistantResult(q.UpdatedBy.Id, q.UpdatedBy.Name),
                    q.CreatedAt,
                    q.UpdatedAt,
                    q.Teacher.Name))
            ;
        return await quizzes
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}


