using TMS.Domain.Common.Models;

namespace TMS.Contracts.Quiz.Get;

public record GetQuizzesResponse(PaginatedList<QuizDto> Quizzes);