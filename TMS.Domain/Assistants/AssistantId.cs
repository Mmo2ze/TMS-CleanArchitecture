﻿namespace TMS.Domain.Assistants;

public record AssistantId(string Value) : ValueObjectId<AssistantId>(Value)
{
	public AssistantId() : this(Ulid.NewUlid().ToString())
	{
	}
}