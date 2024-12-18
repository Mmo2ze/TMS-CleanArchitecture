﻿namespace TMS.Domain.Admins;

public record AdminId(string Value) : ValueObjectId<AdminId>(Value)
{
	public AdminId() : this(Ulid.NewUlid().ToString())
	{
	}
}