using FluentValidation;

namespace TMS.Application.Parents.Queries.Get;

public class GetParentsValidator: AbstractValidator<GetParentsQuery>
{
    public GetParentsValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);
        RuleFor(x => x.Search)
            .Length(2, 50);
        
    }
}