using Mapster;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using TMS.Application.Accounts.Queries.Get;
using TMS.Application.Attendance.Commands.Create;
using TMS.Application.Attendance.Commands.Delete;
using TMS.Application.Attendance.Commands.Update;
using TMS.Application.Attendance.Queries.Get;
using TMS.Contracts.Account.Get.List;
using TMS.Contracts.Attendance;
using TMS.Contracts.Attendance.Create;
using TMS.Contracts.Attendance.Delelte;
using TMS.Contracts.Attendance.Get;
using TMS.Contracts.Attendance.Update;
using TMS.Domain.Accounts;
using TMS.Domain.Attendances;
using TMS.Domain.Common.Models;

namespace TMS.Api.Common.Mapping;

public class AttendanceMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAttendanceRequest, CreateAttendanceCommand>()
            .Map(dest => dest.AccountId, src => AccountId.Create(src.AccountId));

        config.NewConfig<AttendanceResult, AttendanceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<GetAttendancesRequest, GetAttendancesQuery>()
            .Map(dest => dest.AccountId, src => AccountId.Create(src.AccountId));
        config.NewConfig<PaginatedList<AttendanceResult>, PaginatedList<AttendanceResponse>>()
            .ConstructUsing((source,
                context) => new PaginatedList<AttendanceResponse>(
                source.Items.Adapt<IReadOnlyCollection<AttendanceResponse>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));

        config.NewConfig<UpdateAttendanceRequest, UpdateAttendanceCommand>()
            .Map(dest => dest.Id, src => AttendanceId.Create(src.Id));
        config.NewConfig<DeleteAttendanceRequest,DeleteAttendanceCommand>()
            .Map(dest => dest.Id, src => AttendanceId.Create(src.Id))
            .Map(dest => dest.AccountId, src => AccountId.Create(src.AccountId));
    }
}