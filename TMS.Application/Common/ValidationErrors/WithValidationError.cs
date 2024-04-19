using FluentValidation;

namespace TMS.Application.Common.ValidationErrors;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithValidationError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> ruleBuilder,ValidationError error)
    {
        ruleBuilder.WithMessage(error.Description);
        ruleBuilder.WithErrorCode(error.Code);
        ruleBuilder.WithSeverity(Severity.Info);
        return ruleBuilder;
    }
}