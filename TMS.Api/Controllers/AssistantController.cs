using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Assistants.Commands.Create;
using TMS.Application.Authentication.Commands.RefreshToken;
using TMS.Contracts.Assistant.Create;
using IMediator = MassTransit.Mediator.IMediator;

namespace TMS.Api.Controllers;

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
        var response = _mapper.Map<CreateAssistantRequest>(request);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
}