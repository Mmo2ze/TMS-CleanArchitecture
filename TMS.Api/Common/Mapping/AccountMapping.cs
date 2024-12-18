using Mapster;
using TMS.Application.Accounts.Commands.Create;
using TMS.Application.Accounts.Commands.Delete;
using TMS.Application.Accounts.Commands.Update;
using TMS.Application.Accounts.Queries.Get;
using TMS.Application.Accounts.Queries.Get.Details;
using TMS.Contracts.Account.Create;
using TMS.Contracts.Account.Delete;
using TMS.Contracts.Account.DTOs;
using TMS.Contracts.Account.Get.Details;
using TMS.Contracts.Account.Get.List;
using TMS.Contracts.Account.Update;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;
using TMS.Domain.Parents;
using TMS.Domain.Students;

namespace TMS.Api.Common.Mapping;

public class AccountMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAccountRequest, CreateAccountCommand>()
            .Map(dest => dest.GroupId, src => GroupId.Create(src.GroupId))
            .Map(dest => dest.ParentId,
                src => string.IsNullOrWhiteSpace(src.ParentId) ? null : ParentId.Create(src.ParentId))
            .Map(dest => dest.StudentId, src => StudentId.Create(src.StudentId));


        config.NewConfig<GetAccountsRequest, GetAccountsQuery>()
            .Map(dest => dest.GroupId,
                src => !string.IsNullOrWhiteSpace(src.GroupId) ? GroupId.Create(src.GroupId) : null);

        config.NewConfig<UpdateAccountRequest, UpdateAccountCommand>()
            .Map(dest => dest.GroupId,
                src => !string.IsNullOrWhiteSpace(src.GroupId) ? GroupId.Create(src.GroupId) : null)
            .Map(dest => dest.ParentId,
                src => !string.IsNullOrWhiteSpace(src.ParentId) ? ParentId.Create(src.ParentId) : null)
            .Map(dest => dest.StudentId,
                src => !string.IsNullOrWhiteSpace(src.StudentId) ? StudentId.Create(src.StudentId) : null);

        config.NewConfig<UpdateAccountPartialRequest, UpdateAccountPartialCommand>()
            .Map(dest => dest.GroupId,
                src => !string.IsNullOrWhiteSpace(src.GroupId) ? GroupId.Create(src.GroupId) : null)
            .Map(dest => dest.ParentId,
                src => !string.IsNullOrWhiteSpace(src.ParentId) ? ParentId.Create(src.ParentId) : null)
            .Map(dest => dest.StudentId,
                src => !string.IsNullOrWhiteSpace(src.StudentId) ? StudentId.Create(src.StudentId) : null);


        config.NewConfig<AccountSummary, AccountSummaryDto>()
            .Map(dest => dest.AccountId, src => src.AccountId.Value)
            .Map(dest => dest.StudentId, src => src.StudentId.Value)
            .Map(dest => dest.ParentId, src => src.ParentId != null? src.ParentId.Value : null)
            .Map(dest => dest.GroupId, src => src.GroupId.Value);


        config.NewConfig<DeleteAccountRequest, DeleteAccountCommand>()
            .Map(dest => dest.Id, src => AccountId.Create(src.Id));
        


        config.NewConfig<PaginatedList<AccountSummary>, PaginatedList<AccountSummaryDto>>()
            .ConstructUsing((source,
                context) => new PaginatedList<AccountSummaryDto>(
                source.Items.Adapt<IReadOnlyCollection<AccountSummaryDto>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
        config.NewConfig<AccountDetailsResult, AccountDetailsDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.GroupId, src => src.GroupId != null ? src.GroupId.Value : null);
    }
}