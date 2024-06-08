using ErrorOr;
using MediatR;
using TMS.Domain.Account;
using TMS.Domain.Quizzes;

namespace TMS.Application.Quizzes.Commands.Delete;

public record DeleteQuizCommand(QuizId Id,AccountId AccountId): IRequest<ErrorOr<string>>;