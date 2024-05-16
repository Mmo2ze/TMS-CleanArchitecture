using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Application.Accounts.Commands.Create;

public class CreateAccountCommandHandler: IRequestHandler<CreateAccountCommand, ErrorOr<AccountId>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateAccountCommandHandler(IGroupRepository groupRepository, ITeacherHelper teacherHelper, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<AccountId>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FirstAsync(g => g.Id == request.GroupId  , cancellationToken);
        var account = Account.Create(request.StudentId,group.BasePrice,group.Id,group.TeacherId);
        group.AddStudent(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return account.Id;
    }
}