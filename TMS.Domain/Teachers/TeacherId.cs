﻿namespace TMS.Domain.Teachers;

public record TeacherId(string Value) : ValueObjectId<TeacherId>(Value)
{
	public TeacherId() : this(Ulid.NewUlid().ToString())
	{
	}
};