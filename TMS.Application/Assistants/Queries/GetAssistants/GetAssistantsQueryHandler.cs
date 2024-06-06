using ErrorOr;
using MassTransit.Initializers;
using MediatR;
using TMS.Application.Assistants.Commands.Create;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Assistants.Queries.GetAssistants;

public class
    GetAssistantsQueryHandler : IRequestHandler<GetAssistantQuery, ErrorOr<PaginatedList<AssistantDto>>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAssistantRepository _assistantRepository;

    public GetAssistantsQueryHandler(ITeacherHelper teacherHelper, IAssistantRepository assistantRepository)
    {
        _teacherHelper = teacherHelper;
        _assistantRepository = assistantRepository;
    }

    public async Task<ErrorOr<PaginatedList<AssistantDto>>> Handle(GetAssistantQuery request,
        CancellationToken cancellationToken)
    {
        var assistants = _assistantRepository.GetQueryable()
            .Where(x => x.TeacherId == _teacherHelper.GetTeacherId())
            .Select(x => AssistantDto.From(x));
        var result = await assistants.PaginatedListAsync(request.PageNumber, request.PageSize);

        return result;
    }
}