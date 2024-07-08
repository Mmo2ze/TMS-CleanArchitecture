using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Accounts.Commands.Create;
using TMS.Application.Accounts.Commands.Delete;
using TMS.Application.Accounts.Commands.Update;
using TMS.Application.Accounts.Queries.Get;
using TMS.Application.Accounts.Queries.Get.Details;
using TMS.Application.Attendance.Commands.Create;
using TMS.Application.Attendance.Commands.Delete;
using TMS.Application.Attendance.Commands.Update;
using TMS.Application.Attendance.Queries.Get;
using TMS.Application.Payments.Commands.Delete;
using TMS.Application.Payments.Queries.Get;
using TMS.Application.Quizzes.Commands.Create;
using TMS.Application.Quizzes.Commands.Delete;
using TMS.Application.Quizzes.Queries.Get;
using TMS.Contracts.Account.Create;
using TMS.Contracts.Account.DTOs;
using TMS.Contracts.Account.Get.Details;
using TMS.Contracts.Account.Get.List;
using TMS.Contracts.Account.Update;
using TMS.Contracts.Attendance;
using TMS.Contracts.Attendance.Create;
using TMS.Contracts.Attendance.Get;
using TMS.Contracts.Attendance.Update;
using TMS.Contracts.Payments.Create;
using TMS.Contracts.Payments.Get;
using TMS.Contracts.Quiz.Create;
using TMS.Contracts.Quiz.Get;
using TMS.Domain.Accounts;
using TMS.Domain.Attendances;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;
using TMS.Domain.Payments;
using TMS.Domain.Quizzes;

namespace TMS.Api.Controllers;

public class AccountController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public AccountController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        var command = _mapper.Map<CreateAccountCommand>(request);
        var result = await _mediator.Send(command);
        var response = _mapper.Map<AccountSummaryDto>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts([FromQuery] GetAccountsRequest request)
    {
        var query = _mapper.Map<GetAccountsQuery>(request);
        var result = await _mediator.Send(query);
        var response = _mapper.Map<PaginatedList<AccountSummaryDto>>(result.Value);
        return result.Match(
            _ => Ok(response)
            , Problem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountRequest request, string id)
    {
        var command = _mapper.Map<UpdateAccountCommand>(request);
        command = command with { Id = AccountId.Create(id) };

        var result = await _mediator.Send(command);

        var response = _mapper.Map<AccountDetailsDto>(result.Value);

        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAccountPartial([FromQuery] UpdateAccountPartialRequest request, string id)
    {
        var command = _mapper.Map<UpdateAccountPartialCommand>(request);
        command = command with { Id = AccountId.Create(id) };

        var result = await _mediator.Send(command);

        var response = _mapper.Map<AccountSummaryDto>(result.Value);

        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpPost("{id}/quizzes")]
    public async Task<IActionResult> AddQuiz([FromBody] CreateQuizRequest request, string id)
    {
        if (request.AccountId != id)
        {
            return BadRequest("account Id in the request body does not match the account Id in the route.");
        }

        var command = _mapper.Map<CreateQuizCommand>(request);
        command = command with { AccountId = AccountId.Create(id) };

        var result = await _mediator.Send(command);

        return result.Match(
            _ => Ok(result.Value),
            Problem
        );
    }


    [HttpGet("{id}/quizzes")]
    public async Task<IActionResult> GetQuizzes([FromQuery] GetQuizzesRequest request, string id)
    {
        if (request.AccountId != id)
        {
            return BadRequest("account Id in the request body does not match the account Id in the route.");
        }

        var query = _mapper.Map<GetQuizzesQuery>(request);

        var result = await _mediator.Send(query);
        var response = _mapper.Map<PaginatedList<QuizDto>>(result.Value);
        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpDelete("{id}/quiz/{quizId}")]
    public async Task<IActionResult> DeleteQuiz(string id, string quizId)
    {
        var command = new DeleteQuizCommand(QuizId.Create(quizId), AccountId.Create(id));
        var result = await _mediator.Send(command);
        return result.Match(
            _ => Ok(result.Value),
            Problem
        );
    }

    [HttpPost("{id}/attendance")]
    public async Task<IActionResult> AddAttendance([FromBody] CreateAttendanceRequest request, string id)
    {
        if (request.AccountId != id)
        {
            return BadRequest("account Id in the request body does not match the account Id in the route.");
        }

        var command = _mapper.Map<CreateAttendanceCommand>(request);

        var result = await _mediator.Send(command);
        var response = _mapper.Map<AttendanceResponse>(result.Value);

        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpGet("{id}/attendance")]
    public async Task<IActionResult> GetAttendances([FromQuery] GetAttendancesRequest request, string id)
    {
        if (request.AccountId != id)
        {
            return BadRequest("account Id in the request body does not match the account Id in the route.");
        }

        var query = _mapper.Map<GetAttendancesQuery>(request);

        var result = await _mediator.Send(query);
        var response = _mapper.Map<GetAttendancesResponse>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem);
    }

    [HttpPut("attendance/{id}")]
    public async Task<IActionResult> UpdateAttendance([FromBody] UpdateAttendanceRequest request, string id)
    {
        if (request.Id != id)
        {
            return BadRequest("attendance Id in the request body does not match the attendance Id in the route.");
        }

        var command = _mapper.Map<UpdateAttendanceCommand>(request);
        command = command with { Id = AttendanceId.Create(id) };

        var result = await _mediator.Send(command);

        var response = _mapper.Map<AttendanceResponse>(result.Value);

        return result.Match(
            _ => Ok(response),
            Problem
        );
    }

    [HttpDelete("{id}/attendance/{attendanceId}")]
    public async Task<IActionResult> DeleteAttendance(string id, string attendanceId)
    {
        var command = new DeleteAttendanceCommand(AttendanceId.Create(attendanceId), AccountId.Create(id));
        var result = await _mediator.Send(command);
        return result.Match(
            _ => NoContent(),
            Problem
        );
    }
    
    [HttpGet("{id}/payments")]
    public async Task<IActionResult> GetPayments([FromQuery] GetAccountPaymentsRequest request, string id)
    {
        if (request.Id != id)
        {
            return BadRequest("account Id in the request body does not match the account Id in the route.");
        }

        var query = _mapper.Map<GetAccountPaymentsQuery>(request);

        var result = await _mediator.Send(query);
        var response = _mapper.Map<PaginatedList<PaymentDto>>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem);
    }
    
    [HttpDelete("{id}/payments/{paymentId}")]
    public async Task<IActionResult> DeletePayment(string id, string paymentId)
    {
        var command = new DeletePaymentCommand(PaymentId.Create(paymentId), AccountId.Create(id));
        var result = await _mediator.Send(command);
        return result.Match(
            _ => NoContent(),
            Problem
        );
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountDetails(string id)
    {
        var query = new GetAccountDetailsQuery(AccountId.Create(id));
        var result = await _mediator.Send(query);
        var response = _mapper.Map<AccountDetailsDto>(result.Value);
        return result.Match(
            _ => Ok(response),
            Problem
        );
    }
}