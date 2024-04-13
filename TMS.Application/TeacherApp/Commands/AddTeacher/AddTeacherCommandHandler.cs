using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Application.TeacherApp.Queries.GetTeachers;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;

namespace TMS.Application.TeacherApp.Commands.AddTeacher;

public class AddTeacherCommandHandler: IRequestHandler<AddTeacherCommand, ErrorOr<AddTeacherResult>>
{
	private readonly ITeacherRepository _teacherRepository;
	private readonly IWhatsappSender _whatsappSender;

	public AddTeacherCommandHandler(ITeacherRepository teacherRepository, IWhatsappSender whatsappSender)
	{
		_teacherRepository = teacherRepository;
		_whatsappSender = whatsappSender;
	}

	public async Task<ErrorOr<AddTeacherResult>> Handle(AddTeacherCommand request, CancellationToken cancellationToken)
	{
		// Validation
		if(await _teacherRepository.Any(teacher => teacher.Phone == request.Phone, cancellationToken))
			return Errors.Teacher.PhoneAlreadyExists;
		if (request.Email is not null && await _teacherRepository.Any(teacher => teacher.Email == request.Email, cancellationToken))
			return Errors.Teacher.EmailAlreadyExists;
		
		if (!await _whatsappSender.IsValidNumber(request.Phone))
			return Errors.Whatsapp.WhatsappNotInstalled;
		
		// Create teacher 
 		var teacher =  Teacher.Create(request.Name, request.Phone, request.Subject, request.SubscriptionPeriodInDays, request.Email);
		await _teacherRepository.Add(teacher,cancellationToken);
		var summary = new TeacherSummary(teacher.Id, teacher.Name, teacher.Phone, 0,teacher.Subject, teacher.EndOfSubscription);
		var result = new AddTeacherResult(summary);
		return result;
	}
}