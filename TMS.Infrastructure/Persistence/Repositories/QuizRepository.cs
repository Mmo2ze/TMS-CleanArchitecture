using TMS.Domain.Common.Repositories;
using TMS.Domain.Quizzes;

namespace TMS.Infrastructure.Persistence.Repositories;

public class QuizRepository: Repository<Quiz,QuizId> , IQuizRepository
{
    public QuizRepository(MainContext dbContext) : base(dbContext)
    {
    }
}