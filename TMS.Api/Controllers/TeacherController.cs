using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Application.TeacherApp.Commands.AddTeacher;
using TMS.Application.TeacherApp.Commands.UpdateSubscription;
using TMS.Application.TeacherApp.Queries.GetTeacher;
using TMS.Application.TeacherApp.Queries.GetTeachers;
using TMS.Contracts.Teacher.AddTeacher;
using TMS.Contracts.Teacher.GetTeacher;
using TMS.Contracts.Teacher.GetTeachers;
using TMS.Contracts.Teacher.UpdateTeacherSubscrioption;
using TMS.Domain.Admins;
using TMS.Domain.Common.Models;
using TMS.Domain.OutBox;
using TMS.Domain.Teachers;

namespace TMS.Api.Controllers;

[Authorize(Roles = JwtVariables.Roles.AdminR.Role)]
public class TeacherController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IWhatsappSender _sender;

    public TeacherController(IMediator mediator, IMapper mapper, IWhatsappSender sender)
    {
        _mediator = mediator;
        _mapper = mapper;
        _sender = sender;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] GetTeachersRequest request)
    {
        var result = _mediator.Send(_mapper.Map<GetTeachersQuery>(request)).Result;
        var response = _mapper.Map<GetTeachersResponse>(result);
        return Ok(response);
    }

    [HttpPost]
    public IActionResult Post(AddTeacherRequest request)
    {
        var result = _mediator.Send(_mapper.Map<AddTeacherCommand>(request)).Result;
        var response = _mapper.Map<AddTeacherResponse>(result.Value);
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
        var response = _mapper.Map<AddTeacherResponse>(result);
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