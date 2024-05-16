using ErrorOr;

namespace TMS.Domain.Common.Errors;

public static partial class Errors
{
	public static class Whatsapp
	{
		public static Error InvalidPhoneNumber =>
			Error.Validation("Whatsapp.InvalidPhoneNumber", "Invalid phone number");

		public static Error WhatsappNotInstalled =>
			Error.Validation("Whatsapp.WhatsappNotInstalled", "Whatsapp is not installed");

		public static Error WhatsappServiceFailed =>
			Error.Failure("Whatsapp.WhatsappServiceFailed", "Whatsapp service failed");

		public static Error InvalidNumber => 
			Error.Validation("Whatsapp.InvalidNumber", "Invalid number");
	}
}