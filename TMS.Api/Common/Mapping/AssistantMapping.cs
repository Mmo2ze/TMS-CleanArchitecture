using Mapster;
using TMS.Application.Assistants.Commands.Create;
using TMS.Contracts.Assistant.Create;

namespace TMS.Api.Common.Mapping;

public class AssistantMapping:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAssistantResult, CreateAssistantResponse>()
            .Map(dest => dest.Id, src => src.Id);
    }
}