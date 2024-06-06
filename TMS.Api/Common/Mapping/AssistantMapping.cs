using Mapster;
using TMS.Application.Assistants.Commands.Create;
using TMS.Application.Assistants.Commands.Delete;
using TMS.Application.Assistants.Commands.Update;
using TMS.Contracts.Account.DTOs;
using TMS.Contracts.Assistant.Create;
using TMS.Contracts.Assistant.Delete;
using TMS.Contracts.Assistant.Update;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Models;

namespace TMS.Api.Common.Mapping;

public class AssistantMapping:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AssistantDto, CreateAssistantResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
        
        config.NewConfig<PaginatedList<AssistantDto>, PaginatedList<CreateAssistantResponse>>()
            .ConstructUsing((source,
                context) => new PaginatedList<CreateAssistantResponse>(
                source.Items.Adapt<IReadOnlyCollection<CreateAssistantResponse>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
        
        config.NewConfig<DeleteAssistantRequest,DeleteAssistantCommand>()
            .Map(dest => dest.Id, src => AssistantId.Create(src.Id) );
        
        config.NewConfig<UpdateAssistantRequest,UpdateAssistantCommand>()
            .Map(dest => dest.Id, src => AssistantId.Create(src.Id) );
        
    }
    
    
}