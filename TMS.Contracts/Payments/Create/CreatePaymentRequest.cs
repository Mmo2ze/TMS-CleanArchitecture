namespace TMS.Contracts.Payments.Create;

public record CreatePaymentRequest(
    decimal Amount,
    DateOnly BillDate,
    string AccountId
);