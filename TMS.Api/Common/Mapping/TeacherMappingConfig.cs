using Mapster;
using TMS.Application.TeacherApp.Commands.UpdateSubscription;
using TMS.Application.TeacherApp.Queries.GetTeacher;
using TMS.Application.TeacherApp.Queries.GetTeachers;
using TMS.Contracts.Teacher.GetTeacher;
using TMS.Contracts.Teacher.GetTeachers;
using TMS.Contracts.Teacher.UpdateTeacherSubscrioption;
using TMS.Domain.Teachers;

namespace TMS.Api.Common.Mapping;

public class TeacherMappingConfig:IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<TeacherSummary, TeacherSummaryResponse>()
			.Map(dest => dest.Id, src => src.Id.Value);
		config.NewConfig<GetTeacherResult, GetTeacherResponse>()
			.Map(dest => dest.Id, src => src.Id.Value);
		config.NewConfig<UpdateTeacherSubscriptionRequest,UpdateTeacherSubscriptionCommand >()
			.Map(dest => dest.Id , src => TeacherId.Create(src.Id));
		config.NewConfig<UpdateTeacherSubscriptionResult,UpdateTeacherSubscriptionResponse>()
			.Map(dest=> dest.Id , src => src.Id.Value);

	}
}