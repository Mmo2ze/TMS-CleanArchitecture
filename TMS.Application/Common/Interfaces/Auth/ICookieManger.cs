namespace TMS.Application.Common.Interfaces.Auth;

public interface ICookieManger
{
	void SetProperty(string key, object value,TimeSpan period);
	string? GetPropertyByClaimType(string key);
	string GetProperty(string key);
}