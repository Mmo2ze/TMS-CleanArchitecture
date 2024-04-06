namespace TMS.Domain.Assistants;

public record AssistantId(Guid Value)
{
	public static AssistantId Create(Guid value)
	{
		return new AssistantId(value);
	}
	
	public static AssistantId CreateUnique()
	{
		return new AssistantId(Guid.NewGuid());
	}
}