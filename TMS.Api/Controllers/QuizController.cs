using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Quizzes.Commands.Update;
using TMS.Contracts.Quiz.Get;
using TMS.Contracts.Quiz.Update;

namespace TMS.Api.Controllers;

public class QuizController: ApiController
{
 
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public QuizController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuiz([FromBody] UpdateQuizRequest request, string id)
    {
        if(request.Id != id)
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
    
}