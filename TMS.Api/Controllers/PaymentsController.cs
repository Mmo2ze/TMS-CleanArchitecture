using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Common.Variables;
using TMS.Application.Payments.Commands.Create;
using TMS.Application.Payments.Commands.Update;
using TMS.Contracts.Payments.Create;
using TMS.Contracts.Payments.Update;

namespace TMS.Api.Controllers;

[Route("payments")]

public class PaymentsController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PaymentsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.RecordPayment}")]
    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
    {
        var command = _mapper.Map<CreatePaymentCommand>(request);
        var result = await _sender.Send(command);
        var response = _mapper.Map<PaymentDto>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
    
    [Authorize(Roles = $"{Roles.Teacher.Role},{Roles.Assistant.RecordPayment}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayment([FromRoute] string id, [FromBody] UpdatePaymentRequest request)
    {
        if (id != request.Id)
            return BadRequest("Id in route and body are different.");
        var command = _mapper.Map<UpdatePaymentCommand>(request);
        var result = await _sender.Send(command);
        var response = _mapper.Map<PaymentDto>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
}