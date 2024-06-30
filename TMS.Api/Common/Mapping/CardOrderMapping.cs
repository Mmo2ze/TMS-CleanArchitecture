using Mapster;
using TMS.Application.Accounts.Queries.Get.Details;
using TMS.Application.CardOrders.Commands.Create;
using TMS.Contracts.Account.Get.Details;
using TMS.Contracts.CardOrder;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Models;

namespace TMS.Api.Common.Mapping;

public class CardOrderMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, AccountId>()
            .MapWith(src => AccountId.Create(src));
        config.NewConfig<AccountId, string>()
            .MapWith(src => src.Value);
        config.NewConfig<CardOrderResult, CardOrderDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.TeacherId, src => src.TeacherId.Value)
            .Map(dest => dest.AcceptedBy, src => src.AcceptedBy.Value)
            .Map(dest => dest.CancelledBy, src => src.CancelledBy.Value);
        
        config.NewConfig<PaginatedList<CardOrderResult>, PaginatedList<CardOrderDto>>()
            .ConstructUsing((source,
                context) => new PaginatedList<CardOrderDto>(
                source.Items.Adapt<IReadOnlyCollection<CardOrderDto>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
        config.NewConfig<AccountDetailsResult, AccountDetailsDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.GroupId, src => src.GroupId != null ? src.GroupId.Value : null);
    }
}