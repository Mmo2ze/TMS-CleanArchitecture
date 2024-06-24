using Mapster;
using TMS.Application.Quizzes.Commands.Create;
using TMS.Application.Quizzes.Commands.Update;
using TMS.Application.Quizzes.Queries.Get;
using TMS.Contracts.Quiz.Create;
using TMS.Contracts.Quiz.Get;
using TMS.Contracts.Quiz.Update;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Models;
using TMS.Domain.Quizzes;

namespace TMS.Api.Common.Mapping;

public class QuizMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateQuizRequest, CreateQuizCommand>()
            .Map(dest => dest.AccountId, src => AccountId.Create(src.AccountId));

        config.NewConfig<GetQuizzesRequest, GetQuizzesQuery>()
            .Map(dest => dest.AccountId, src => AccountId.Create(src.AccountId));
        config.NewConfig<QuizResult, QuizDto>()
            .Map(dest => dest.Id, src => src.Id.Value);


        config.NewConfig<PaginatedList<QuizResult>, PaginatedList<QuizDto>>()
            .ConstructUsing((source,
                context) => new PaginatedList<QuizDto>(
                source.Items.Adapt<IReadOnlyCollection<QuizDto>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));

        config.NewConfig<QuizAssistantResult, QuizAssistantResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<UpdateQuizRequest, UpdateQuizCommand>()
            .Map(dest => dest.Id, src => QuizId.Create(src.Id));
    }
}