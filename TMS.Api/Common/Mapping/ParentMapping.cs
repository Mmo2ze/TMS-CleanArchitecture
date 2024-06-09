using Mapster;
using Microsoft.Win32;
using TMS.Application.Parents.Queries.Get;
using TMS.Contracts.Parent.Get;
using TMS.Domain.Common.Models;

namespace TMS.Api.Common.Mapping;

public class ParentMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        
        config.NewConfig<ParentResult, ParentDto>()
            .Map(dest => dest.Id, src => src.Id.Value);
        
        config.NewConfig<PaginatedList<ParentResult>, PaginatedList<ParentDto>>()
            .ConstructUsing((source,
                context) => new PaginatedList<ParentDto>(
                source.Items.Adapt<IReadOnlyCollection<ParentDto>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
    }
}