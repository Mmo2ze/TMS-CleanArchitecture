using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Application.Teachers.Commands.Create;
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
using TMS.Infrastructure.Persistence;
using TMS.MessagingContracts;

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
        var result = _mediator.Send(_mapper.Map<GetTeachersQuery>(request)).Result;
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
    public async Task<IActionResult> Get(string id)
    {
        var request = new GetTeacherRequest(id);
        var result = await _mediator.Send(_mapper.Map<GetTeacherQuery>(request));
        var response = _mapper.Map<CreateTeacherResponse>(result);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateTeacherRequest request)
    {
        request = request with { TeacherId = id };
        var command = _mapper.Map<UpdateTeacherCommand>(request);
        var result = await _mediator.Send(_mapper.Map<UpdateTeacherCommand>(command));
        var response = _mapper.Map<UpdateTeacherResponse>(result);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
    
    [HttpPatch("subscription/{id}")]
    public async Task<IActionResult> UpdateSubscription(string id ,UpdateTeacherSubscriptionRequest request)
    {
        request = request with { Id = id };
        var command = _mapper.Map<UpdateTeacherSubscriptionCommand>(request);
        var result = await _mediator.Send(_mapper.Map<UpdateTeacherSubscriptionCommand>(command));
        var response = _mapper.Map<UpdateTeacherSubscriptionResponse>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
    
    
    


    
}