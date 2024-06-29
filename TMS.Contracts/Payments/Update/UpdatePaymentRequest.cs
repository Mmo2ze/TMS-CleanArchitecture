namespace TMS.Contracts.Payments.Update;

public record UpdatePaymentRequest(string Id, decimal Amount, DateOnly BillDate);
