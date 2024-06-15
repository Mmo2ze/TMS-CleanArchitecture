using MassTransit;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Consumer.MessageTemplates;
using TMS.Infrastructure.Persistence;
using TMS.MessagingContracts.Quiz;

namespace TMS.Consumer.Consumers.Quizzes;

public class QuizUpdatedConsumer: IConsumer<QuizUpdatedEvent>
{
    private readonly MainContext _dbContext;
    private readonly IWhatsappSender _whatsappSender;

    public QuizUpdatedConsumer(MainContext dbContext, IWhatsappSender whatsappSender)
    {
        this._dbContext = dbContext;
        _whatsappSender = whatsappSender;
    }

    public async Task Consume(ConsumeContext<QuizUpdatedEvent> context)
    {
        var account = _dbContext.Accounts
            .Where(x => x.Id == context.Message.AccountId)
            .Include(x => x.Parent)
            .Include(x => x.Student)
            .Select(account => new
            {
                x = account, Parent = account.Parent == null ? null : new { account.Parent.Phone, account.Parent.Name },
                account.Student.Name
            }).FirstOrDefault();
        if (account?.Parent == null)
        {
            return;
        }

        var updatedBy =
            _dbContext.Quizzes
                .Where(x => x.Id == context.Message.QuizId)
                .Include(x => x.UpdatedBy)
                .Include(x => x.Teacher)
                .Select(x => new
                {
                    Name = x.UpdatedBy == null ? x.Teacher.Name : x.UpdatedBy.Name,
                    x.Teacher
                })
                .FirstOrDefault();
        if (updatedBy == null)
        {
            return;
        }
        
        var message = MsgTemplate.Quiz.Updated(
            context.Message.Degree,
            context.Message.MaxDegree,
            account.Parent.Name,
            account.Name,
            updatedBy.Name
        );
        
        await _whatsappSender.Send(account.Parent.Phone!, message, updatedBy.Teacher);
    }
}