using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Common.Variables;
using TMS.Application.Students.Commands.Create;
using TMS.Application.Students.Queries.GetStudents;
using TMS.Contracts.Student.Create;
using TMS.Contracts.Student.Get;

namespace TMS.Api.Controllers;

[Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Admin.Role}")]
public class StudentController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public StudentController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStudentRequest request)
    {
        var command = _mapper.Map<CreateStudentCommand>(request);
        var result = _mediator.Send(command).Result;
        return result.Match(
            _ => Ok(result.Value.Value),
            Problem
        );
    }
    
    [HttpGet] 
    public IActionResult Get([FromQuery]GetStudentsRequest request)
    {
        var query = _mapper.Map<GetStudentsQuery>(request);
        var result = _mediator.Send(query).Result;
        var response = _mapper.Map<GetStudentResponse>(result);
        return Ok(result.Value);
    }

}