using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Attendance.Commands.Delete;

public class DeleteAttendanceHandler : IRequestHandler<DeleteAttendanceCommand, ErrorOr<string>>
{
    private readonly IAccountRepository _accountRepository;
    public DeleteAttendanceHandler( IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<ErrorOr<string>> Handle(DeleteAttendanceCommand request, CancellationToken cancellationToken)
    {
        var account = _accountRepository.GetQueryable()
            .Include(x => x.Attendances.Where(a => a.Id == request.Id))
            .First(x => x.Id == request.AccountId);
        var attendance  = account.Attendances.First(x => x.Id == request.Id);
        account.RemoveAttendance(attendance);
        
        return "Attendance deleted successfully.";
    }
}