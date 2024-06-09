using ErrorOr;
using MediatR;
using TMS.Application.Common.Mapping;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Parents;

namespace TMS.Application.Parents.Queries.Get;

public class GetParentsQueryHandler : IRequestHandler<GetParentsQuery, ErrorOr<PaginatedList<ParentResult>>>
{
    private readonly IParentRepository _parentRepository;

    public GetParentsQueryHandler(IParentRepository parentRepository)
    {
        _parentRepository = parentRepository;
    }

    public async Task<ErrorOr<PaginatedList<ParentResult>>> Handle(GetParentsQuery request,
        CancellationToken cancellationToken)
    {
        var parents = _parentRepository.GetQueryable()
            .Where(x =>
                string.IsNullOrWhiteSpace(request.Search) ||
                x.Name.Contains(request.Search) ||
                x.Email.Contains(request.Search) ||
                x.Phone.Contains(request.Search) ||
                x.Id == new ParentId(request.Search)).Select(
                x => new ParentResult(x.Id, x.Name, x.Email, x.Phone)
            );
        var result = await parents.PaginatedListAsync(request.PageNumber, request.PageSize);
        return result;
    }
}