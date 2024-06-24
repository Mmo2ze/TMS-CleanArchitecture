using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Commands.Delete;

public record DeleteQuizCommand(QuizId Id,AccountId AccountId): IRequest<ErrorOr<string>>;