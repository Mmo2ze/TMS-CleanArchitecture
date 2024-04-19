using ErrorOr;
using MassTransit;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Application.Teachers.Queries.GetTeachers;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;
using TMS.MessagingContracts;
using TMS.MessagingContracts.Teacher;

namespace TMS.Application.Teachers.Commands.Create;

public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, ErrorOr<CreateTeacherResult>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IPublishEndpoint _bus;
    private readonly IWhatsappSender _whatsappSender;

    public CreateTeacherCommandHandler(ITeacherRepository teacherRepository, IPublishEndpoint bus,
        IWhatsappSender whatsappSender)
    {
        _teacherRepository = teacherRepository;
        _bus = bus;
        _whatsappSender = whatsappSender;
    }

    public async Task<ErrorOr<CreateTeacherResult>> Handle(CreateTeacherCommand request,
        CancellationToken cancellationToken)
    {
        // Validation
        var adminPhone = "201004714938";




        // Create teacher 
        var teacher = Teacher.Create(request.Name, request.Phone, request.Subject, request.SubscriptionPeriodInDays,
            request.Email);
        await _teacherRepository.AddAsync(teacher, cancellationToken);
        
        
        await _bus.Publish(new TeacherCreatedEvent(
            teacher.Phone,
            teacher.Name,
            teacher.EndOfSubscription, adminPhone), cancellationToken);
        await _teacherRepository.SaveChangesAsync(cancellationToken);
        var summary = TeacherSummary.FromTeacher(teacher);
        return new CreateTeacherResult(summary);;
    }
}