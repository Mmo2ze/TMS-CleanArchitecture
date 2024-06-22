using ErrorOr;
using FluentValidation;
using FluentValidation.Validators;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Models;

namespace TMS.Application.Common.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> ruleBuilder, ValidationError error)
    {
        ruleBuilder.WithMessage(error.Description);
        ruleBuilder.OverridePropertyName(error.Code);
        ruleBuilder.WithErrorCode(ErrorType.Validation.ToString());

        ruleBuilder.WithSeverity(Severity.Info);
        return ruleBuilder;
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> ruleBuilder, Error error)
    {
        ruleBuilder.WithMessage(error.Description);
        ruleBuilder.OverridePropertyName(error.Code);
        ruleBuilder.WithErrorCode(error.Type.ToString());
        return ruleBuilder;
    }


    public static IRuleBuilderOptions<T, string> Length<T>(this IRuleBuilder<T, string> ruleBuilder, Length length)
        => ruleBuilder.SetValidator(new LengthValidator<T>(length.Min, length.Max));
}