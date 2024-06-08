namespace TMS.Contracts.Quiz.Update;

public record UpdateQuizRequest(string Id, int Degree, int MaxDegree);