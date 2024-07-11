using ErrorOr;
using MediatR;
using TMS.Application.Quizzes.Queries.Get;
using TMS.Domain.Accounts;
using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Commands.Create;

public record CreateQuizCommand(
    AccountId AccountId,
    double Degree,
    double MaxDegree,
    string? GroupId) : IRequest<ErrorOr<QuizResult>>;