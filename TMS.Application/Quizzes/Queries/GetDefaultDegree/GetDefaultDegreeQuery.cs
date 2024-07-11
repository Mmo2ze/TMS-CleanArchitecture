using ErrorOr;
using MediatR;

namespace TMS.Application.Quizzes.Queries.GetDefaultDegree;

public record GetDefaultDegreeQuery():IRequest<ErrorOr<GetDefaultDegreeResult>>;

public record GetDefaultDegreeResult(double DefaultDegree);