using Mapster;
using TMS.Application.Students.Commands.Create;
using TMS.Contracts.Student.Create;

namespace TMS.Api.Common.Mapping;

public class StudentMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateStudentRequest, CreateStudentCommand>();
    }
}