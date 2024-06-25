using Mapster;
using TMS.Contracts.AttendanceScheduler.Create;
using TMS.Domain.AttendanceSchedulers;
using TMS.Domain.Common.Models;

namespace TMS.Api.Common.Mapping;

public class AttendanceSchedulerMapping:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AttendanceScheduler, AttendanceSchedulerResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
        config.NewConfig<PaginatedList<AttendanceScheduler>, PaginatedList<AttendanceSchedulerResponse>>()
            .ConstructUsing((source,
                context) => new PaginatedList<AttendanceSchedulerResponse>(
                source.Items.Adapt<IReadOnlyCollection<AttendanceSchedulerResponse>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
        
    }
    
    
    
}