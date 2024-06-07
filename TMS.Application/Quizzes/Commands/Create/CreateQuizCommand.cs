using ErrorOr;
using MediatR;
using TMS.Domain.Account;
using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Create;

public record CreateQuizCommand(
    AccountId AccountId,
    double Degree,
    double MaxDegree,
    string? GroupId) : IRequest<ErrorOr<QuizId>>;