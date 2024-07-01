using FluentValidation;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.CardOrders.Commands.Create;

public class CreateCardOrderValidator : AbstractValidator<CreateCardOrderCommand>
{
    public CreateCardOrderValidator(ITeacherHelper teacherHelper, IAccountRepository accountRepository)
    {
        var teacherId = teacherHelper.GetTeacherId();
        RuleFor(x => x.AccountIds)
            .MustAsync(async (ids, token) =>
            {
                var existingAccountIds = await accountRepository
                    .WhereQueryable(a => ids.Contains(a.Id))
                    .Select(x => x.Id)
                    .ToListAsync();

                return ids.All(id => existingAccountIds.Contains(id));
            })
            .WithError(Errors.CardOrder.AccountsNotFound);
    }
}