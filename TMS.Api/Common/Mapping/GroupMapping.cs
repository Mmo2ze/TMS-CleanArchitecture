using Mapster;
using TMS.Application.Groups.Commands.Update;
using TMS.Application.Groups.Queries.GetGroups;
using TMS.Contracts.Group.GetGroups;
using TMS.Contracts.Group.Update;
using TMS.Domain.Common.Models;

namespace TMS.Api.Common.Mapping;

public class GroupMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaginatedList<GetGroupResult>, PaginatedList<GetGroupResponse>>()
            .ConstructUsing((source,
                context) => new PaginatedList<GetGroupResponse>(
                source.Items.Adapt<IReadOnlyCollection<GetGroupResponse>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));

        config.NewConfig<GetGroupResult, GetGroupResponse>()
            .Map(dest => dest.GroupId, src => src.GroupId.Value);

        config.NewConfig<UpdateGroupResult, UpdateGroupResponse>()
            .Map(dest => dest.GroupId, src => src.Id.Value);
    }
}