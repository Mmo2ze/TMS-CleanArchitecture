using Mapster;
using TMS.Contracts.AttendanceScheduler.Create;
using TMS.Domain.AttendanceSchedulers;
using TMS.Domain.Common.Models;

namespace TMS.Api.Common.Mapping;

public class AttendanceSchedulerMapping:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Scheduler, AttendanceSchedulerResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
        config.NewConfig<PaginatedList<Scheduler>, PaginatedList<AttendanceSchedulerResponse>>()
            .ConstructUsing((source,
                context) => new PaginatedList<AttendanceSchedulerResponse>(
                source.Items.Adapt<IReadOnlyCollection<AttendanceSchedulerResponse>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
        
    }
    
    
    
}