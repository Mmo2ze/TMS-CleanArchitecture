using Mapster;
using TMS.Application.Teachers.Commands.UpdateSubscription;
using TMS.Application.Teachers.Queries.GetTeacher;
using TMS.Contracts.Teacher.Common;
using TMS.Contracts.Teacher.GetTeacher;
using TMS.Contracts.Teacher.UpdateTeacherSubscrioption;
using TMS.Domain.Common.Models;
using TMS.Domain.Teachers;

namespace TMS.Api.Common.Mapping;

public class TeacherMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TeacherSummary, TeacherSummaryResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
        config.NewConfig<PaginatedList<TeacherSummary>, PaginatedList<TeacherSummaryResponse>>()
            .ConstructUsing((source,
                context) => new PaginatedList<TeacherSummaryResponse>(
                source.Items.Adapt<IReadOnlyCollection<TeacherSummaryResponse>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
        
        config.NewConfig<GetTeacherResult, GetTeacherResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
        config.NewConfig<GetTeacherResult, GetTeacherResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
        config.NewConfig<UpdateTeacherSubscriptionRequest, UpdateTeacherSubscriptionCommand>()
            .Map(dest => dest.Id, src => TeacherId.Create(src.Id));
        config.NewConfig<UpdateTeacherSubscriptionResult, UpdateTeacherSubscriptionResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}