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
    private readonly IPublishEndpoint _publishEndpoint;

    public AddTeacherCommandHandler(ITeacherRepository teacherRepository, IPublishEndpoint publishEndpoint)
    {
        _teacherRepository = teacherRepository;
        _publishEndpoint = publishEndpoint;
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
        await _teacherRepository.Add(teacher, cancellationToken);
        await _teacherRepository.SaveChanges(cancellationToken);
        await _publishEndpoint.Publish(new TeacherCreatedEvent(
            teacher.Id.Value,
            teacher.Name,
            teacher.Phone));

        var summary = new TeacherSummary(teacher.Id, teacher.Name, teacher.Phone, 0, teacher.Subject,
            teacher.EndOfSubscription);
        var result = new AddTeacherResult(summary);
        return result;
    }
}