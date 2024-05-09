namespace TMS.MessagingContracts.Authentication;

public record VerificationCodeCreatedEvent(string Code,string Phone);