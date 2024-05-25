namespace TMS.Contracts.Account.Update;

public record UpdateAccountRequest(string Id, string StudentId, string GroupId, double BasePrice);