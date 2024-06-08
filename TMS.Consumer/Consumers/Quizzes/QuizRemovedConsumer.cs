using MassTransit;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Consumer.MessageTemplates;
using TMS.MessagingContracts.Quiz;

namespace TMS.Consumer.Consumers.Quizzes;

public class QuizRemovedConsumer : IConsumer<QuizRemovedEvent>
{
    private readonly MainContext _dbContext;
    private readonly IWhatsappSender _whatsappSender;

    public QuizRemovedConsumer(IWhatsappSender whatsappSender, MainContext dbContext)
    {
        _whatsappSender = whatsappSender;
        _dbContext = dbContext;
    }

    public Task Consume(ConsumeContext<QuizRemovedEvent> context)
    {
        var account = _dbContext.Accounts
            .Include(x => x.Parent)
            .Select(x => new
            {
                x.Id,
                x.TeacherId,
                Parent = x.Parent == null ? null : new { x.Parent.Phone, x.Parent.Name },
                x.Student.Name
            })
            .FirstOrDefault(x => x.Id == context.Message.AccountId);
        if (account?.Parent == null)
        {
            return Task.CompletedTask;
        }

        var teacher = _dbContext.Teachers
            .First(x => x.Id == account.TeacherId);

        var message = MsgTemplate.Quiz.Removed(account.Parent.Name, account.Name);
        return _whatsappSender.Send(account.Parent.Phone!, message, teacher);
    }
}