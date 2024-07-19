using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Common.Variables;
using TMS.Application.Quizzes.Commands.Update;
using TMS.Application.Quizzes.Commands.UpdateDefaultDegree;
using TMS.Application.Quizzes.Queries.GetDefaultDegree;
using TMS.Contracts.Quiz.Get;
using TMS.Contracts.Quiz.Update;

namespace TMS.Api.Controllers;

[Route("quiz")]
[Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.Role}")]

public class QuizController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public QuizController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.AddQuiz}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuiz([FromBody] UpdateQuizRequest request, string id)
    {
        if (request.Id != id)
        {
            return BadRequest("Id mismatch");
        }

        var command = _mapper.Map<UpdateQuizCommand>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<QuizDto>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpGet("default-degree")]
    public async Task<IActionResult> GetDefaultDegree()
    {
        var query = new GetDefaultDegreeQuery();
        var results = await _mediator.Send(query);
        return results.Match(
            x => Ok(results.Value),
            Problem
        );
    }
    
    [Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.AddQuiz}")]
    [HttpPut("default-degree")]
    public async Task<IActionResult> UpdateDefaultDegree([FromBody] UpdateDefaultDegreeCommand request)
    {
        var result = await _mediator.Send(request);
        return result.Match(
            _ => NoContent(),
            Problem
        );
    }
}