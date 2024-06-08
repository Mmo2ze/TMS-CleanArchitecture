using ErrorOr;
using MediatR;
using TMS.Domain.Account;
using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Commands.Create;

public record CreateQuizCommand(
    AccountId AccountId,
    double Degree,
    double MaxDegree,
    string? GroupId) : IRequest<ErrorOr<QuizId>>;