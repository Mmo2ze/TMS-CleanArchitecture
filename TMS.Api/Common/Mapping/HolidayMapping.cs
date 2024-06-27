using Mapster;
using TMS.Application.Holidays.Commands.Create;
using TMS.Contracts.Holiday.Create;
using TMS.Domain.Groups;

namespace TMS.Api.Common.Mapping;

public class HolidayMapping:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateHolidayRequest,CreateHolidayCommand>()
            .Map(dest => dest.GroupId, src => src.GroupId == null ? null : new GroupId(src.GroupId));
        config.NewConfig<HolidayResult, HolidayDto>()
            .Map(dest => dest.GroupId, src => src.GroupId == null ? null : src.GroupId.Value)
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}