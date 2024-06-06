using ErrorOr;
using MediatR;
using TMS.Application.Assistants.Commands.Create;
using TMS.Application.Common;
using TMS.Domain.Account;
using TMS.Domain.Common.Models;

namespace TMS.Application.Assistants.Queries.GetAssistants;

public record GetAssistantQuery(
    int PageNumber = 1,
    int PageSize = 10
) : GetPaginatedList(PageNumber, PageSize),
    IRequest<ErrorOr<PaginatedList<AssistantDto>>>;