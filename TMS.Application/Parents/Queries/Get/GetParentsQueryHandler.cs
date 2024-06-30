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
        var parents = _parentRepository.GetQueryable();

        if (request.PhoneRequired)
            parents = parents.Where(x => !string.IsNullOrWhiteSpace(x.Phone));
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            // filter by phone if required

            // search by name, email, phone or id
            parents = parents.Where(x =>
                x.Name.Contains(request.Search) ||
                x.Email.Contains(request.Search) ||
                x.Phone.Contains(request.Search) ||
                x.Id == new ParentId(request.Search));
        }
        
        var result = await parents
            .OrderBy(x => x.Name)
            .Select(x => new ParentResult(x.Id, x.Name, x.Email, x.Phone, x.Gender, x.HasWhatsapp))
            .PaginatedListAsync(request.Page, request.PageSize);
        return result;
    }
}