using ErrorOr;
using MassTransit;
using MediatR;
using TMS.Application.TeacherApp.Queries.GetTeachers;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;
using TMS.MessagingContracts;

namespace TMS.Application.TeacherApp.Commands.AddTeacher;

public class AddTeacherCommandHandler : IRequestHandler<AddTeacherCommand, ErrorOr<AddTeacherResult>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IPublishEndpoint _bus;

    public AddTeacherCommandHandler(ITeacherRepository teacherRepository, IPublishEndpoint bus )
    {
        _teacherRepository = teacherRepository;
        _bus = bus;
    }

    public async Task<ErrorOr<AddTeacherResult>> Handle(AddTeacherCommand request, CancellationToken cancellationToken)
    {
        // Validation

        // if(await _teacherRepository.Any(teacher => teacher.Phone == request.Phone, cancellationToken))
        // 	return Errors.Teacher.PhoneAlreadyExists;
        // if (request.Email is not null && await _teacherRepository.Any(teacher => teacher.Email == request.Email, cancellationToken))
        // 	return Errors.Teacher.EmailAlreadyExists;
        //
        // if (!await _whatsappSender.IsValidNumber(request.Phone))
        // 	return Errors.Whatsapp.WhatsappNotInstalled;

        // Create teacher 
        var teacher = Teacher.Create(request.Name, request.Phone, request.Subject, request.SubscriptionPeriodInDays,
            request.Email);
        await _teacherRepository.AddAsync(teacher, cancellationToken);
        await _bus.Publish(new TeacherCreatedEvent(
            teacher.Id.Value,
            teacher.Name,
            teacher.Phone), cancellationToken);
        await _teacherRepository.SaveChangesAsync(cancellationToken);
        var summary = new TeacherSummary(teacher.Id, teacher.Name, teacher.Phone, 0, teacher.Subject,
            teacher.EndOfSubscription);
        var result = new AddTeacherResult(summary);
        return result;
    }
}