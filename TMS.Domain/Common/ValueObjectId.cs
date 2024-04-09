namespace TMS.Domain.Common;

public record ValueObjectId<T>(string Value)
{
	public static T Create(string value)
	{
		var valueWithOutDashes = value.Replace("-", "");
		var newValue = GetPrefixedId() + valueWithOutDashes;
		
		return (T)Activator.CreateInstance(typeof(T), newValue)!;
	}

	public static T CreateUnique()
	{
		return Create(Guid.NewGuid().ToString());
	}

	private static string GetPrefixedId()
	{
		var typeName = typeof(T).Name.ToLower();
		return typeName.Length >= 2 ? typeName[..2]+"_" : typeName+"_";
	}
}