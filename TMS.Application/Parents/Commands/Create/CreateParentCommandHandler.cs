using ErrorOr;
using MediatR;
using TMS.Application.Parents.Queries.Get;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Parents;

namespace TMS.Application.Parents.Commands.Create;

public class CreateParentCommandHandler : IRequestHandler<CreateParentCommand, ErrorOr<ParentResult>>
{
    private readonly IParentRepository _parentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateParentCommandHandler(IParentRepository parentRepository, IUnitOfWork unitOfWork)
    {
        _parentRepository = parentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ParentResult>> Handle(CreateParentCommand request, CancellationToken cancellationToken)
    {
        var parent = Parent.Create(request.Name, request.Gender, request.Email, request.Phone);
        _parentRepository.Add(parent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new ParentResult(parent.Id, parent.Name,parent.Email,parent.Phone,parent.Gender);
    }
}