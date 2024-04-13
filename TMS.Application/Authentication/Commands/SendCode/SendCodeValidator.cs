using FluentValidation;

namespace TMS.Application.Authentication.Commands.SendCode;

public class SendCodeValidator: AbstractValidator<SendCodeCommand>
{
	public SendCodeValidator()
	{
		RuleFor(x => x.Phone)
			.NotNull()
			.MaximumLength(16)
			.MinimumLength(9);
		RuleFor(x => x.UserAgent)
			.NotNull();
	}	
}