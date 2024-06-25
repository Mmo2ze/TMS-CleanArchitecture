using MediatR;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Common.Behaviors;

public class UnitOfWorkBehavior<TRequest,TResponse>: 
    IPipelineBehavior<TRequest,TResponse> 
    where TRequest : notnull 
{
    private readonly IUnitOfWork _unitOfWork;
    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(IsNotCommand())
        {
            return await next();
        }
        var response = await next();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return response;
    }

    private static bool IsNotCommand()
    {
        return !typeof(TRequest).Name.EndsWith("Command");
    }
}