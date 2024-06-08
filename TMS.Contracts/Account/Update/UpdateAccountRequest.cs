namespace TMS.Contracts.Account.Update;

public record UpdateAccountRequest( string StudentId, string GroupId, double BasePrice, string ParentId);