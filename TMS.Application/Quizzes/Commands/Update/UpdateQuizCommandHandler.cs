using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Application.Quizzes.Queries.Get;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Quizzes.Commands.Update;

public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, ErrorOr<QuizResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateQuizCommandHandler(ITeacherHelper teacherHelper, IQuizRepository quizRepository,
        IUnitOfWork unitOfWork)
    {
        _teacherHelper = teacherHelper;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<QuizResult>> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetQueryable()
            .Where(q => q.Id == request.Id && q.TeacherId == _teacherHelper.GetTeacherId())
            .Select(q => new
            {
                Quiz = q,
                TeacherName = q.Teacher.Name,
                AddedBy = q.AddedBy == null ? null : new { q.AddedBy.Name, q.AddedBy.Id },
                UpdatedBy = q.UpdatedBy == null ? null :new { q.UpdatedBy.Name, q.UpdatedBy.Id
            }
        })
        .FirstAsync(cancellationToken);


        quiz.Quiz.Update(request.Degree, request.MaxDegree, _teacherHelper.GetAssistantId());

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new QuizResult(
            quiz.Quiz.Id,
            quiz.Quiz.Degree,
            quiz.Quiz.MaxDegree,
            quiz.AddedBy == null ? null : new QuizAssistantResult(quiz.AddedBy.Id, quiz.AddedBy.Name),
            quiz.UpdatedBy == null ? null : new QuizAssistantResult(quiz.UpdatedBy.Id, quiz.UpdatedBy.Name),
            quiz.Quiz.CreatedAt,
            quiz.Quiz.UpdatedAt,
            quiz.TeacherName);
    }
}