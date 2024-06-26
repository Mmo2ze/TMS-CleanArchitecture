using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Scheduler.Commands.Create;
using TMS.Contracts.AttendanceScheduler.Create;

namespace TMS.Api.Controllers;


public class SchedulerController:ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SchedulerController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("auto")]
    public async Task<IActionResult> CreateAutoScheduler(CreateAutoAttendanceSchedulerRequest request)
    {
        var command = new CreateAutoSchedulerCommand(request.SchedulerOption);
        var result = await _mediator.Send(command);
        var response  = _mapper.Map<List<AttendanceSchedulerResponse>>(result.Value);
        return result.Match(
            success => Ok(response),
            errors => Problem()
        );
    }
}