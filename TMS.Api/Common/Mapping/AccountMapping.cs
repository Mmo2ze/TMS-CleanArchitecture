using Mapster;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Accounts.Commands.Create;
using TMS.Application.Accounts.Commands.Update;
using TMS.Application.Accounts.Queries.Get;
using TMS.Contracts.Account.Create;
using TMS.Contracts.Account.DTOs;
using TMS.Contracts.Account.Get.List;
using TMS.Contracts.Account.Update;
using TMS.Contracts.Teacher.Common;
using TMS.Contracts.Teacher.Create;
using TMS.Domain.Account;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;
using TMS.Domain.Students;

namespace TMS.Api.Common.Mapping;

public class AccountMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAccountRequest, CreateAccountCommand>()
            .Map(dest => dest.GroupId, src => GroupId.Create(src.GroupId))
            .Map(dest => dest.StudentId, src => StudentId.Create(src.StudentId));


        config.NewConfig<GetAccountsRequest, GetAccountsQuery>()
            .Map(dest => dest.GroupId, src => GroupId.Create(src.GroupId));

        config.NewConfig<UpdateAccountRequest, UpdateAccountCommand>()
            .Map(dest => dest.Id, src => AccountId.Create(src.Id))
            .Map(dest => dest.GroupId, src => GroupId.Create(src.GroupId))
            .Map(dest => dest.StudentId, src => StudentId.Create(src.StudentId));

        config.NewConfig<AccountSummary, AccountSummaryDto>()
            .Map(dest => dest.AccountId, src => src.AccountId.Value)
            .Map(dest => dest.StudentId, src => src.StudentId.Value)
            .Map(dest => dest.GroupId, src => src.GroupId.Value);


        config.NewConfig<PaginatedList<AccountSummary>, PaginatedList<AccountSummaryDto>>()
            .ConstructUsing((source,
                context) => new PaginatedList<AccountSummaryDto>(
                source.Items.Adapt<IReadOnlyCollection<AccountSummaryDto>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
    }
}