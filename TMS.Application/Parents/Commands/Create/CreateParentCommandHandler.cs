using ErrorOr;
using MediatR;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Parents;

namespace TMS.Application.Parents.Commands.Create;

public class CreateParentCommandHandler : IRequestHandler<CreateParentComamnd, ErrorOr<ParentId>>
{
    private readonly IParentRepository _parentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateParentCommandHandler(IParentRepository parentRepository, IUnitOfWork unitOfWork)
    {
        _parentRepository = parentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ParentId>> Handle(CreateParentComamnd request, CancellationToken cancellationToken)
    {
        var parent = Parent.Create(request.Name, request.Gender, request.Email, request.Phone);
        _parentRepository.Add(parent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return parent.Id;
    }
}