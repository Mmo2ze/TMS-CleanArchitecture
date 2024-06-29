using Mapster;
using TMS.Application.Payments.Commands.Create;
using TMS.Application.Payments.Commands.Update;
using TMS.Application.Payments.Queries.Get;
using TMS.Contracts.Payments.Create;
using TMS.Contracts.Payments.Get;
using TMS.Contracts.Payments.Update;
using TMS.Domain.Accounts;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Models;
using TMS.Domain.Payments;

namespace TMS.Api.Common.Mapping;

public class PaymentMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaymentResult, PaymentDto>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<CreatePaymentRequest, CreatePaymentCommand>()
            .Map(dest => dest.AccountId, src => AccountId.Create(src.AccountId));

        config.NewConfig<UpdatePaymentRequest, UpdatePaymentCommand>()
            .Map(dest => dest.Id, src => PaymentId.Create(src.Id));

        config.NewConfig<GetAccountPaymentsRequest, GetAccountPaymentsQuery>()
            .Map(dest => dest.Id, src => AccountId.Create(src.Id));
        config.NewConfig<AssistantInfo, AssistantInfoDto>()
            .Map(dest => dest.Id, src => src.Id != null ? src.Id.Value : null);
        config.NewConfig<PaginatedList<PaymentResult>, PaginatedList<PaymentDto>>()
            .ConstructUsing((source,
                context) => new PaginatedList<PaymentDto>(
                source.Items.Adapt<IReadOnlyCollection<PaymentDto>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
    }
}