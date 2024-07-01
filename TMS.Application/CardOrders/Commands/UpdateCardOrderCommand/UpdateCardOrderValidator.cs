using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Domain.Cards;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.CardOrders.Commands.UpdateCardOrderCommand;

public class UpdateCardOrderValidator : AbstractValidator<UpdateCardOrderCommand>
{
    private readonly IClaimsReader _claimsReader;
    private readonly ICardOrderRepository _cardOrderRepository;

    public UpdateCardOrderValidator(IClaimsReader claimsReader, ICardOrderRepository cardOrderRepository)
    {
        _claimsReader = claimsReader;
        _cardOrderRepository = cardOrderRepository;
        var roles = claimsReader.GetRoles();
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Ids)
            .Must(ids => roles.Contains("Teacher") || roles.Contains("Assistant")).When(x => x.Ids != null)
            .WithMessage("Only teacher or assistant can change ids");
        RuleFor(x => x.Status)
            .Must(status =>
            {
                if (status == CardOrderStatus.Processing || status == CardOrderStatus.Completed)
                {
                    return roles.Contains(Roles.Admin.Role);
                }

                return true;
            })
            .WithMessage(
                "Only admin can change status to processing or completed you can change status to pending or cancelled");
        

    }

    private static Func<UpdateCardOrderCommand, bool> IsTeacherOrAssistant(List<string> roles)
    {
        return x => roles.Contains(Roles.Teacher.Role) || roles.Contains(Roles.Assistant.AddCardOrder);
    }

    private static Func<UpdateCardOrderCommand, bool> IsAdmin(List<string> roles)
    {
        return (x) => roles.Contains(Roles.Admin.Role);
    }
}