using ErrorOr;
using MediatR;
using TMS.Application.Quizzes.Queries.Get;
using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Commands.Update;

public record UpdateQuizCommand(QuizId Id, int Degree, int MaxDegree) : IRequest<ErrorOr<QuizResult>>;