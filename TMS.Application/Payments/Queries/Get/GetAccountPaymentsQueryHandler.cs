using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Application.Payments.Commands.Create;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Payments.Queries.Get;

public class
    GetAccountPaymentsQueryHandler : IRequestHandler<GetAccountPaymentsQuery, ErrorOr<PaginatedList<PaymentResult>>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IPaymentRepository _paymentRepository;

    public GetAccountPaymentsQueryHandler(ITeacherHelper teacherHelper, IPaymentRepository paymentRepository)
    {
        _teacherHelper = teacherHelper;
        _paymentRepository = paymentRepository;
    }

    public async Task<ErrorOr<PaginatedList<PaymentResult>>> Handle(GetAccountPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        var payments = _paymentRepository.WhereQueryable(x=> x.TeacherId == _teacherHelper.GetTeacherId() && x.AccountId == request.Id)
            .Include(x => x.CreatedBy)
            .Include(x => x.ModifiedBy)
            .Select(x => new PaymentResult(
                x.Id,
                x.Amount,
                x.BillDate,
                x.CreatedBy != null
                    ? new AssistantInfo(x.CreatedBy.Name, x.CreatedById)
                    : _teacherHelper.TeacherInfo(),
                x.CreatedAt,
                x.ModifiedBy != null
                    ? new AssistantInfo(x.ModifiedBy.Name, x.CreatedById)
                    : _teacherHelper.TeacherInfo(),
                x.ModifiedAt
            ));
        return await payments.PaginatedListAsync(request.Page, request.PageSize);

    }
}