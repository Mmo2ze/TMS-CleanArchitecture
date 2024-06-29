using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Commands.Delete;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, ErrorOr<string>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAccountCommandHandler(ITeacherHelper teacherHelper, IAccountRepository accountRepository,
        IUnitOfWork unitOfWork, IGroupRepository groupRepository)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _groupRepository = groupRepository;
    }

    public async Task<ErrorOr<string>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        
        
        var group = await _groupRepository.GetQueryable()
            .Include(x => x.Accounts.Where(s => s.Id == request.Id))
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken: cancellationToken);


        group!.RemoveStudent(group.Accounts[0]);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "deleted Successfully";
    }
}