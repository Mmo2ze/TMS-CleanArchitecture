using Mapster;
using TMS.Application.Students.Commands.Create;
using TMS.Application.Students.Queries.GetStudents;
using TMS.Contracts.Student.Create;
using TMS.Contracts.Student.Get;
using TMS.Domain.Common.Models;

namespace TMS.Api.Common.Mapping;

public class StudentMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateStudentRequest, CreateStudentCommand>();
        config.NewConfig<StudentResult, StudentDto>()
            .Map(dest => dest.Id, src => src.Id.Value);
        config.NewConfig<PaginatedList<StudentResult>, PaginatedList<StudentDto>>()
            .ConstructUsing((source,
                context) => new PaginatedList<StudentDto>(
                source.Items.Adapt<IReadOnlyCollection<StudentDto>>(),
                source.TotalCount,
                source.PageNumber,
                source.GetPageSize()));
    }
}