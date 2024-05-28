namespace TMS.Contracts.Account.Update;

public record UpdateAccountPartialRequest(string? StudentId, string? GroupId, double? BasePrice);