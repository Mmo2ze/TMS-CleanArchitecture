using TMS.Contracts.Session.Get;

namespace TMS.Contracts.Session.Create;

public record CreateSessionResponse(string SessionId,SessionResponseSummary SessionSummary);