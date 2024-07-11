using Mapster;
using TMS.Application.Sessions.Commands.Create;
using TMS.Application.Sessions.Queries.Get;
using TMS.Contracts.Session.Create;
using TMS.Contracts.Session.Get;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;
using TMS.Domain.Sessions;

namespace TMS.Api.Common.Mapping;

public class SessionMapping:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateSessionRequest, CreateSessionCommand>()
            .Map(dest => dest.GroupId, src => GroupId.Create(src.GroupId));
        
        
        config.NewConfig<PaginatedList<Session>, PaginatedList<SessionResponseSummary>>()
            .ConstructUsing((source,
                context) => new PaginatedList<SessionResponseSummary>(
                source.Items.Adapt<IReadOnlyCollection<SessionResponseSummary>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
        
    config.NewConfig<GetSessionsRequest,GetSessionsQuery>()
        .Map(dest => dest.GroupId, src => src.GroupId != null ? GroupId.Create(src.GroupId) : null);
    
        config.NewConfig<Session, SessionResponseSummary>()
            .Map(dest => dest.GroupId, src => src.GroupId.Value)
            .Map(dest => dest.Id, src => src.Id.Value);


    }
}