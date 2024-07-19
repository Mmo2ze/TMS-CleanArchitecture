using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Assistants.Commands.Create;
using TMS.Application.Assistants.Commands.Delete;
using TMS.Application.Assistants.Commands.Update;
using TMS.Application.Assistants.Queries.GetAssistants;
using TMS.Application.Common.Variables;
using TMS.Contracts.Assistant.Create;
using TMS.Contracts.Assistant.Delete;
using TMS.Contracts.Assistant.Get;
using TMS.Contracts.Assistant.Update;
using TMS.Domain.Common.Models;

namespace TMS.Api.Controllers;


[Authorize(Roles = $"{Roles.Teacher.Role}")]
public class AssistantController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AssistantController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAssistant([FromBody] CreateAssistantRequest request)
    {
        var command = _mapper.Map<CreateAssistantCommand>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<CreateAssistantResponse>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAssistant([FromQuery] GetAssistantRequest request)
    {
        var command = _mapper.Map<GetAssistantQuery>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<PaginatedList<CreateAssistantResponse>>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
    
    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateAssistant([FromBody] UpdateAssistantRequest request)
    {
        var command = _mapper.Map<UpdateAssistantCommand>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<AssistantDto>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteAssistant([FromRoute] DeleteAssistantRequest request)
    {
        var command = _mapper.Map<DeleteAssistantCommand>(request);
        var result = await _mediator.Send(command);
        return result.Match(
            _ => NoContent(),
            Problem
        );
    }
}