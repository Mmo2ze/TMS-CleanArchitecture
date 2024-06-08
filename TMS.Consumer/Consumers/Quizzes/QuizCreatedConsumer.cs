using MassTransit;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Consumer.MessageTemplates;
using TMS.Domain.Common.Repositories;
using TMS.MessagingContracts.Quiz;

namespace TMS.Consumer.Consumers.Quizzes;

public class QuizCreatedConsumer : IConsumer<QuizCreatedEvent>
{
    private readonly IWhatsappSender _whatsappSender;
    private readonly MainContext _dbContext;

    public QuizCreatedConsumer(IWhatsappSender whatsappSender, MainContext dbContext)
    {
        _whatsappSender = whatsappSender;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<QuizCreatedEvent> context)
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

        var AddedBy =
            _dbContext.Quizzes
                .Where(x => x.Id == context.Message.QuizId)
                .Include(x => x.AddedBy)
                .Include(x => x.Teacher)
                .Select(x => new
                {
                    Name = x.AddedBy == null ? x.Teacher.Name : x.AddedBy.Name,
                    x.Teacher
                })
                .FirstOrDefault();
        if (AddedBy == null)
        {
            return;
        }

        var message = MsgTemplate.Quiz.Created(
            context.Message.Degree,
            context.Message.MaxDegree,
            account.Parent.Name,
            account.Name,
            AddedBy.Name
        );
        await _whatsappSender.Send(account.Parent.Phone!, message, AddedBy.Teacher);
    }
}