namespace TMS.Contracts.Payments.Get;

public record GetAccountPaymentsRequest(int Page, int PageSize, string Id);