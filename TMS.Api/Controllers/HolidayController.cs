using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Common.Variables;
using TMS.Application.Holidays.Commands.Create;
using TMS.Application.Holidays.Commands.Delete;
using TMS.Application.Holidays.Commands.Update;
using TMS.Application.Holidays.Queries.Get;
using TMS.Contracts.Holiday.Create;
using TMS.Contracts.Holiday.Get;
using TMS.Contracts.Holiday.Update;
using TMS.Domain.Common.Models;
using TMS.Domain.Holidays;

namespace TMS.Api.Controllers;

[Route("holiday")]
[Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.Role}")]
public class HolidayController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public HolidayController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.AddHoliday}")]

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
    
    [Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.AddHoliday}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHoliday([FromRoute] string id, [FromBody] UpdateHolidayRequest request)
    {
        if (id != request.Id)
            return BadRequest("Id in the route and in the body do not match.");
        var command = _mapper.Map<UpdateHolidayCommand>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<HolidayDto>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
    
    [Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.AddHoliday}")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHoliday([FromRoute] string id)
    {
        var command = new DeleteHolidayCommand(HolidayId.Create(id));
        var result = await _mediator.Send(command);
        return result.Match(
            _ => NoContent(),
            Problem
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetHolidays([FromQuery] GetHolidaysRequest request)
    {
        var query = _mapper.Map<GetHolidaysQuery>(request);
        var result = await _mediator.Send(query);
        var response = _mapper.Map<PaginatedList<HolidayDto>>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
}