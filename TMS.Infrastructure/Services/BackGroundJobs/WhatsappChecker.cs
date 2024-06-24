using Coravel.Invocable;
using TMS.Application.Common.Services;
using TMS.Infrastructure.Persistence;

namespace TMS.Infrastructure.Services.BackGroundJobs;

public class WhatsappCheckerJob : IInvocable
{
    private readonly MainContext _dbContext;
    private readonly IWhatsappSender _whatsappSender;

    public WhatsappCheckerJob(MainContext dbContext, IWhatsappSender whatsappSender)
    {
        _dbContext = dbContext;
        _whatsappSender = whatsappSender;
    }



    public async Task Invoke()
    {
        var admins = _dbContext.Admins.Where(a => a.HasWhatsapp == null).Take(20).ToList();
        foreach (var admin in admins)
        {
            admin.HasWhatsapp = await _whatsappSender.IsValidNumber(admin.Phone); 
        }
        var teachers = _dbContext.Teachers.Where(a => a.HasWhatsapp == null).Take(20).ToList();
        foreach (var teacher in teachers)
        {
            teacher.HasWhatsapp = await _whatsappSender.IsValidNumber(teacher.Phone); 
        }
        var students = _dbContext.Students.Where(a => a.HasWhatsapp == null&&a.Phone != null).Take(100).ToList();
        foreach (var student in students)
        {
            student.HasWhatsapp = await _whatsappSender.IsValidNumber(student.Phone!); 
        }
        var parents = _dbContext.Parents.Where(a => a.HasWhatsapp == null && a.Phone!= null).Take(100).ToList();
        foreach (var parent in parents)
        {
            parent.HasWhatsapp = await _whatsappSender.IsValidNumber(parent.Phone!); 
        }
        await _dbContext.SaveChangesAsync();
    }
}