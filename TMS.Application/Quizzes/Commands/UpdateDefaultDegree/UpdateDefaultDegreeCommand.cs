using ErrorOr;
using MediatR;
using TMS.Application.Quizzes.Queries.GetDefaultDegree;

namespace TMS.Application.Quizzes.Commands.UpdateDefaultDegree;

public record UpdateDefaultDegreeCommand(double DefaultDegree) : IRequest<ErrorOr<GetDefaultDegreeResult>>;