using Mapster;
using TMS.Application.Sessions;
using TMS.Contracts.Session.Create;
using TMS.Domain.Groups;

namespace TMS.Api.Common.Mapping;

public class SessionMapping:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateSessionRequest, CreateSessionCommand>()
            .Map(dest => dest.GroupId, src => GroupId.Create(src.GroupId));
        
        

    }
}