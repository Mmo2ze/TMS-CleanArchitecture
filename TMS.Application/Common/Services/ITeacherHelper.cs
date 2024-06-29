using TMS.Domain.Assistants;
using TMS.Domain.Teachers;

namespace TMS.Application.Common.Services;

public interface ITeacherHelper
{
    bool IsTeacher();
    bool IsAssistant();
    TeacherId GetTeacherId();
    AssistantId? GetAssistantId();
    AssistantInfo GetAssistantInfo();

    string GetTeacherName();
    AssistantInfo TeacherInfo();
}