using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Holidays.Commands.Create;
using TMS.Contracts.Holiday.Create;

namespace TMS.Api.Controllers;

[Route("holiday")]
public class HolidayController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public HolidayController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateHoliday([FromBody] CreateHolidayRequest request)
    {
        var command = _mapper.Map<CreateHolidayCommand>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<HolidayDto>(result.Value);
        return result.Match(
            _ => Ok(response),
             Problem
        );
    }
}