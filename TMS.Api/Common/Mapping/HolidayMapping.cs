using Mapster;
using TMS.Application.Holidays.Commands.Create;
using TMS.Application.Holidays.Commands.Delete;
using TMS.Application.Holidays.Commands.Update;
using TMS.Application.Holidays.Queries.Get;
using TMS.Contracts.Holiday.Create;
using TMS.Contracts.Holiday.Delete;
using TMS.Contracts.Holiday.Get;
using TMS.Contracts.Holiday.Update;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;

namespace TMS.Api.Common.Mapping;

public class HolidayMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateHolidayRequest, CreateHolidayCommand>()
            .Map(dest => dest.GroupId, src => src.GroupId == null ? null : new GroupId(src.GroupId));
        config.NewConfig<HolidayResult, HolidayDto>()
            .Map(dest => dest.GroupId, src => src.GroupId == null ? null : src.GroupId.Value)
            .Map(dest => dest.Id, src => src.Id.Value);
        config.NewConfig<UpdateHolidayRequest, UpdateHolidayCommand>()
            .Map(dest => dest.GroupId, src => src.GroupId == null ? null : new GroupId(src.GroupId))
            .Map(dest => dest.Id, src =>  Domain.Holidays.HolidayId.Create(src.Id));
        config.NewConfig<DeleteHolidayRequest, DeleteHolidayCommand>()
            .Map(dest => dest.Id, src =>  Domain.Holidays.HolidayId.Create(src.Id));
        config.NewConfig<GetHolidaysRequest, GetHolidaysQuery>()
            .Map(dest => dest.GroupId, src => src.GroupId != null ? GroupId.Create(src.GroupId) : null);
        
        config.NewConfig<PaginatedList<HolidayResult>, PaginatedList<HolidayDto>>()
            .ConstructUsing((source,
                context) => new PaginatedList<HolidayDto>(
                source.Items.Adapt<IReadOnlyCollection<HolidayDto>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
    }
}