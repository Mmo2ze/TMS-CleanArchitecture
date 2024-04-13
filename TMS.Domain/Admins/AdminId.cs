﻿using TMS.Domain.Common;
using TMS.Domain.Common.Models;

namespace TMS.Domain.Admins;

public record AdminId(string Value) : ValueObjectId<AdminId>(Value)
{
	public AdminId() : this(Guid.NewGuid().ToString())
	{
	}
}