using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Common.Variables;
using TMS.Application.Teachers.Commands.Create;
using TMS.Application.Teachers.Commands.Delete;
using TMS.Application.Teachers.Commands.Update;
using TMS.Application.Teachers.Commands.UpdateSubscription;
using TMS.Application.Teachers.Queries.GetTeacher;
using TMS.Application.Teachers.Queries.GetTeachers;
using TMS.Contracts.Teacher.Create;
using TMS.Contracts.Teacher.GetTeacher;
using TMS.Contracts.Teacher.GetTeachers;
using TMS.Contracts.Teacher.Update;
using TMS.Contracts.Teacher.UpdateTeacherSubscrioption;
using TMS.Domain.Teachers;

namespace TMS.Api.Controllers;

[Authorize(Roles = JwtVariables.Roles.AdminR.Role)]
public class TeacherController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TeacherController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] GetTeachersRequest request)
    {
        var query = _mapper.Map<GetTeachersQuery>(request);
        var result = _mediator.Send(query).Result;
        var response = _mapper.Map<GetTeachersResponse>(result);
        return Ok(response);
    }

    [HttpPost]
    public IActionResult Post(CreateTeacherRequest request)
    {
        var command = _mapper.Map<CreateTeacherCommand>(request);
        var result = _mediator.Send(command).Result;
        var response = _mapper.Map<CreateTeacherResponse>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpGet("{id}")]
    public Task<IActionResult> Get(string id)
    {
        var query = new GetTeacherQuery(TeacherId.Create(id));
        var result = _mediator.Send(query).Result;
        var response = _mapper.Map<GetTeacherResponse>(result.Value);
        return Task.FromResult(result.Match(
            _ => Ok(response),
            Problem
        ));
    }

    [HttpPut("{id}")]
    public Task<IActionResult> Update(string id, UpdateTeacherRequest request)
    {
        var command = _mapper.Map<UpdateTeacherCommand>(request);
        command = command with { TeacherId = TeacherId.Create(id) };
        var result =  _mediator.Send(_mapper.Map<UpdateTeacherCommand>(command)).Result;
        var response = _mapper.Map<UpdateTeacherResponse>(result);
        return Task.FromResult(result.Match(
            _ => Ok(response),
            Problem
        ));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdatePatch(string id, [FromQuery] UpdateTeacherPartialRequest request)
    {
        var command = _mapper.Map<UpdateTeacherPartialCommand>(request);
        command = command with { TeacherId = TeacherId.Create(id) };
        var result = await _mediator.Send(command);
        var response = _mapper.Map<UpdateTeacherResponse>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpPatch("subscription/{id}")]
    public async Task<IActionResult> UpdateSubscription(string id, UpdateTeacherSubscriptionRequest request)
    {
        var command = _mapper.Map<UpdateTeacherSubscriptionCommand>(request);
        command = command with { Id = TeacherId.Create(id) };
        var result = await _mediator.Send(command);
        var response = _mapper.Map<UpdateTeacherSubscriptionResponse>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var command = new DeleteTeacherCommand(TeacherId.Create(id));
        var result = await _mediator.Send(command);
        return result.Match(
            _ => NoContent(),
            Problem
        );
    }
    
    
}