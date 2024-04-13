namespace TMS.Domain.Common.Models;

public record ValueObjectId<T>(string Value)
{
	public static T Create(string value)
	{
		
		return (T)Activator.CreateInstance(typeof(T), value)!;
	}

	public static T CreateUnique()
	{
		var value = Guid.NewGuid().ToString();
		var valueWithOutDashes = value.Replace("-", "");
		var newValue = GetPrefixedId() + valueWithOutDashes;
		return Create(newValue);
	}

	private static string GetPrefixedId()
	{
		var typeName = typeof(T).Name.ToLower();
		return typeName.Length >= 2 ? typeName[..2]+"_" : typeName+"_";
	}
}