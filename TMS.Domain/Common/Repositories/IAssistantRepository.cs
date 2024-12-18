﻿using TMS.Domain.Assistants;

namespace TMS.Domain.Common.Repositories;

public interface IAssistantRepository:IRepository<Assistant,AssistantId>
{
	Task<bool> IsAssistant(string phone);
	Task<Assistant?> GetAssistantByPhone(string phone);
}